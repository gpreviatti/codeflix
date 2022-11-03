using Application.Dtos.Genre;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;
using DomainEntity = Domain.Entity;

namespace Application.UseCases.Genre;
public class CreateGenre : ICreateGenre
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenre(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository
    )
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse<GenreOutput>> Handle(
        CreateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var genre = new DomainEntity.Genre(
            request.Name,
            request.Is_Active
        );
        if ((request.Categories_Ids?.Count ?? 0) > 0)
        {
            await ValidateCategoriesIds(request, cancellationToken);
            request.Categories_Ids?.ForEach(genre.AddCategory);
        }
        
        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new(GenreOutput.FromGenre(genre));
    }

    private async Task ValidateCategoriesIds(
        CreateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var IdsInPersistence = await _categoryRepository
            .GetIdsListByIds(
                request.Categories_Ids!,
                cancellationToken
            );
        if (IdsInPersistence.Count < request.Categories_Ids!.Count)
        {
            var notFoundIds = request.Categories_Ids
                .FindAll(x => !IdsInPersistence.Contains(x));
            var notFoundIdsAsString = String.Join(", ", notFoundIds);
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}
