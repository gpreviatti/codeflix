using Application.Dtos.Common;

namespace Application.Dtos.Category;

public class ListCategoriesOutput : PaginatedListOutput<CategoryOutput>
{
    public ListCategoriesOutput(
        int page,
        int per_page,
        int filtred,
        int total,
        IReadOnlyList<CategoryOutput> items
    ) : base(page, per_page, filtred, total, items) { }
}
