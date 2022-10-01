using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Domain.Repository;

namespace Application.UseCases.Category;

public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(
        ListCategoriesInput request,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _categoryRepository.Search(
            new(
                request.Page,
                request.Per_Page,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        var items = searchOutput.Items.Select(CategoryOutput.FromCategory).ToList();

        return new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Filtred,
            searchOutput.Total,
            items
        );
    }
}
