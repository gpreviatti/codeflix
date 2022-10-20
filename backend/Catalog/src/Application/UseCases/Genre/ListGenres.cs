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

    public async Task<BasePaginatedResponse<List<GenreOutput>>> Handle(
        ListGenresInput input,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _genreRepository.Search(
            input.ToSearchInput(), cancellationToken
        );

        var genres = searchOutput.Items.Select(GenreOutput.FromGenre).ToList();

        var relatedCategoriesIds = genres.SelectMany(item => item.Categories).Distinct().ToList();

        if (relatedCategoriesIds.Count > 0)
        {
            var categories = await _categoryRepository.GetListByIds(
                relatedCategoriesIds.Select(r => r.Id).ToList(), 
                cancellationToken
            );

            foreach (var genre in genres)
                foreach (var categoryOutput in genre.Categories)
                    categoryOutput.Name = categories.FirstOrDefault(category => category.Id == categoryOutput.Id)?.Name;
        }

        return new(
            genres,
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Filtred,
            searchOutput.Total
        );
    }
}
