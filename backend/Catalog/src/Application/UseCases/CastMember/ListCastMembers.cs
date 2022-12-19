using Application.Dtos.CastMember;
using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.CastMember;

public class ListCastMembers : IListCastMembers
{
    private readonly ICastMemberRepository _repository;

    public ListCastMembers(ICastMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<BasePaginatedResponse<List<CastMemberOutput>>> Handle(
        ListCastMembersInput request,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _repository.Search(
            new(
                request.Page,
                request.Per_Page,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        var items = searchOutput.Items.Select(CastMemberOutput.FromCastMember).ToList();

        return new(
            items,
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Filtred,
            searchOutput.Total
        );
    }
}
