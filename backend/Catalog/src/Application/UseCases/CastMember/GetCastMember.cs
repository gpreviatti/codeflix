using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.CastMember;

public class GetCastMember : IGetCastMember
{
    private readonly ICastMemberRepository _repository;

    public GetCastMember(ICastMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<CastMemberOutput>> Handle(
        GetCastMemberInput input,
        CancellationToken cancellationToken
    )
    {
        var entity = await _repository.Get(input.Id, cancellationToken);

        return new(CastMemberOutput.FromCastMember(entity));
    }
}
