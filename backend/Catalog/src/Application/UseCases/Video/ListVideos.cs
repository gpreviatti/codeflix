using Application.Dtos.Video;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;
using DomainEntity = Domain.Entity;

namespace Application.UseCases.Video;
public class ListVideos : IListVideos
{
    private readonly IVideoRepository _videoRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;

    public ListVideos(
        IVideoRepository videoRepository,
        ICategoryRepository categoryRepository,
        IGenreRepository genreRepository
    )
    {
        _videoRepository = videoRepository;
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
    }

    public async Task<BasePaginatedResponse<List<VideoOutput>>> Handle(
        ListVideosInput input,
        CancellationToken cancellationToken)
    {
        var result = await _videoRepository.Search(input.ToSearchInput(), cancellationToken);

        IReadOnlyList<DomainEntity.Category>? categories = null;
        
        var relatedCategoriesIds = result.Items
            .SelectMany(video => video.Categories).Distinct().ToList();

        if (relatedCategoriesIds is not null && relatedCategoriesIds.Count > 0)
            categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);

        IReadOnlyList<DomainEntity.Genre>? genres = null;
        var relatedGenresIds = result.Items.SelectMany(item => item.Genres).Distinct().ToList();
        if (relatedGenresIds is not null && relatedGenresIds.Count > 0)
            genres = await _genreRepository.GetListByIds(relatedGenresIds, cancellationToken);

        return new(
            result.Items.Select(item => VideoOutput.FromVideo(item, categories, genres)).ToList(),
            result.CurrentPage,
            result.PerPage,
            result.Filtred,
            result.Total
        );
    }
}
