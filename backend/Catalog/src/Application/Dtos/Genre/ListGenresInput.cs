using Application.Dtos.Common;
using Application.Messages;
using Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Application.Dtos.Genre;
public class ListGenresInput : PaginatedListInput, IRequest<BasePaginatedResponse<List<GenreOutput>>>
{
    public ListGenresInput(
    int page = 1,
    int perPage = 15,
    string search = "",
    string sort = "",
    SearchOrder dir = SearchOrder.Asc
    ) : base(page, perPage, search, sort, dir) { }
}
