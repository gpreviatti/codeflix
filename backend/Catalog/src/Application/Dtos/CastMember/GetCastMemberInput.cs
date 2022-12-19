using Application.Messages;
using MediatR;

namespace Application.Dtos.CastMember;
public class GetCastMemberInput : IRequest<BaseResponse<CastMemberOutput>>
{
    public Guid Id { get; private set; }
    public GetCastMemberInput(Guid id) => Id = id;
}
