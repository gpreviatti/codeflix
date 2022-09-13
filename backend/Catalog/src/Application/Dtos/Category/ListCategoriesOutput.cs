using Application.Dtos.Common;

namespace Application.Dtos.Category;

public class ListCategoriesOutput : PaginatedListOutput<CategoryOutput>
{
    public ListCategoriesOutput(
        int page,
        int perPage,
        int filtred,
        int total,
        IReadOnlyList<CategoryOutput> items
    ) : base(page, perPage, filtred, total, items) { }
}
