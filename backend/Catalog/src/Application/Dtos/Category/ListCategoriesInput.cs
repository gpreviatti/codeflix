using Application.Dtos.Common;
using Application.Messages;
using Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Application.Dtos.Category;

public class ListCategoriesInput : PaginatedListInput, IRequest<BasePaginatedResponse<List<CategoryOutput>>>
{
    public ListCategoriesInput(
        int page = 1,
        int per_page = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc
    ) : base(page, per_page, search, sort, dir) { }

    public ListCategoriesInput() : base(1, 15, "", "", SearchOrder.Asc) { }
}
