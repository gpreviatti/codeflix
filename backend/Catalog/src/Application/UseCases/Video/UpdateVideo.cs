using Application.Dtos.Common;
using Application.Dtos.Video;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Excpetions;
using Domain.Repository;
using Domain.Validation;
using DomainEntity = Domain.Entity;

namespace Application.UseCases.Video;
public class UpdateVideo : IUpdateVideo
{
    private readonly IVideoRepository _videoRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICastMemberRepository _castMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public UpdateVideo(
        IVideoRepository videoRepository,
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository,
        ICastMemberRepository castMemberRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _videoRepository = videoRepository;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
        _castMemberRepository = castMemberRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<VideoOutput> Handle(
        UpdateVideoInput input,
        CancellationToken cancellationToken)
    {
        var video = await _videoRepository.Get(input.Id, cancellationToken);
        
        video.Update(
            input.Title,
            input.Description,
            input.YearLaunched,
            input.Opened,
            input.Published,
            input.Duration,
            input.Rating
        );

        var validationHandler = new NotificationValidationHandler();
        video.Validate(validationHandler);

        if (validationHandler.HasErrors())
            throw new EntityValidationException("There are validation errors");

        await ValidateAndAddRelations(input, video, cancellationToken);

        await UploadImagesMedia(video, input, cancellationToken);

        await _videoRepository.Update(video, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return VideoOutput.FromVideo(video);
    }

    private async Task ValidateAndAddRelations(
        UpdateVideoInput input,
        DomainEntity.Video video,
        CancellationToken cancellationToken)
    {
        if (input.GenresIds is not null)
        {
            video.RemoveAllGenres();
            if (input.GenresIds.Count > 0)
            {
                await ValidateGenresIds(input, cancellationToken);
                input.GenresIds!.ToList().ForEach(video.AddGenre);
            }
        }

        if (input.CategoriesIds is not null)
        {
            video.RemoveAllCategories();
            if (input.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(input, cancellationToken);
                input.CategoriesIds!.ToList().ForEach(video.AddCategory);
            }
        }

        if (input.CastMembersIds is not null)
        {
            video.RemoveAllCastMembers();
            if (input.CastMembersIds.Count > 0)
            {
                await ValidateCastMembersIds(input, cancellationToken);
                input.CastMembersIds!.ToList().ForEach(video.AddCastMember);
            }
        }
    }

    private async Task ValidateGenresIds(UpdateVideoInput input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _genreRepository.GetIdsListByIds(
            input.GenresIds!.ToList(), cancellationToken);

        if (persistenceIds.Count < input.GenresIds!.Count)
        {
            var notFoundIds = input.GenresIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related genre id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCategoriesIds(UpdateVideoInput input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _categoryRepository.GetIdsListByIds(
            input.CategoriesIds!.ToList(), cancellationToken);
        if (persistenceIds.Count < input.CategoriesIds!.Count)
        {
            var notFoundIds = input.CategoriesIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCastMembersIds(UpdateVideoInput input, CancellationToken cancellationToken)
    {
        var persistenceIds = await _castMemberRepository.GetIdsListByIds(
            input.CastMembersIds!.ToList(), cancellationToken);
        if (persistenceIds.Count < input.CastMembersIds!.Count)
        {
            var notFoundIds = input.CastMembersIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            throw new RelatedAggregateException(
                $"Related cast member(s) id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task UploadImagesMedia(
        DomainEntity.Video video,
        UpdateVideoInput input,
        CancellationToken cancellationToken)
    {
        if (input.Banner is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Banner), input.Banner.Extension);
            var bannerUrl = await _storageService.Upload(
            fileName,
                input.Banner.FileStream,
                cancellationToken);
            video.UpdateBanner(bannerUrl);
        }

        if (input.Thumb is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Thumb), input.Thumb.Extension);
            var thumbUrl = await _storageService.Upload(
            fileName,
                input.Thumb.FileStream,
                cancellationToken);
            video.UpdateThumb(thumbUrl);
        }

        if (input.ThumbHalf is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.ThumbHalf), input.ThumbHalf.Extension);
            var thumbUrl = await _storageService.Upload(
            fileName,
                input.ThumbHalf.FileStream,
                cancellationToken);
            video.UpdateThumbHalf(thumbUrl);
        }
    }
}
