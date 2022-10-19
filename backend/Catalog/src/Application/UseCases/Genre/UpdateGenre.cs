using Application.Dtos.Genre;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Genre;
public class UpdateGenre : IUpdateGenre
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenre(
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
        UpdateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        genre.Update(request.Name);
        
        if (request.IsActive is not null && request.IsActive != genre.IsActive)
        {
            if ((bool)request.IsActive) genre.Activate();
            else genre.Deactivate();
        }

        if (request.CategoriesIds is not null)
        {
            genre.RemoveAllCategories();
            if (request.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }
        }
        
        await _genreRepository.Update(genre, cancellationToken);
        
        await _unitOfWork.Commit(cancellationToken);

        return new(GenreOutput.FromGenre(genre));
    }

    private async Task ValidateCategoriesIds(
        UpdateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var IdsInPersistence = await _categoryRepository.GetIdsListByIds(
            request.CategoriesIds!,
            cancellationToken
        );

        if (IdsInPersistence.Count < request.CategoriesIds!.Count)
        {
            var notFoundIds = request.CategoriesIds
                .FindAll(x => !IdsInPersistence.Contains(x));
            var notFoundIdsAsString = String.Join(", ", notFoundIds);
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}
