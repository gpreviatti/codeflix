using MediatR;

namespace Application.Dtos.CastMember;
public class DeleteCastMemberInput : IRequest
{
    public Guid Id { get; private set; }
    public DeleteCastMemberInput(Guid id) => Id = id;
}
