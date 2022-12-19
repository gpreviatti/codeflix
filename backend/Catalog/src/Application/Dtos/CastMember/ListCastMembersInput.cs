using Application.Dtos.Common;
using Application.Messages;
using Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Application.Dtos.CastMember;
public class ListCastMembersInput : PaginatedListInput, IRequest<BasePaginatedResponse<List<CastMemberOutput>>>
{
    public ListCastMembersInput(
    int page = 1,
    int per_page = 15,
    string search = "",
    string sort = "",
    SearchOrder dir = SearchOrder.Asc
) : base(page, per_page, search, sort, dir) { }

    public ListCastMembersInput() : base(1, 15, "", "", SearchOrder.Asc) { }
}
