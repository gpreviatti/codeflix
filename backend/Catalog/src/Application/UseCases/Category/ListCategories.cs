using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Category;

public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BasePaginatedResponse<List<CategoryOutput>>> Handle(
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
            items,
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Filtred,
            searchOutput.Total
        );
    }
}
