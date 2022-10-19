using Application.Dtos.Category;
using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Genre;

public class ListGenres : IListGenres
{
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ListGenres(
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository
    ) => (_genreRepository, _categoryRepository) = (genreRepository, categoryRepository);

    public async Task<BasePaginatedResponse<ListGenresOutput>> Handle(
        ListGenresInput input,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _genreRepository.Search(
            input.ToSearchInput(), cancellationToken
        );

        var items = searchOutput.Items.Select(GenreOutput.FromGenre).ToList();

        var relatedCategoriesIds = items.SelectMany(item => item.Categories).Distinct().ToList();

        if (relatedCategoriesIds.Count > 0)
        {
            var categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);

            items.FillWithCategoryNames(categories);
        }

        return new(
            items,
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Filtred,
            searchOutput.Total
        );
    }
}
