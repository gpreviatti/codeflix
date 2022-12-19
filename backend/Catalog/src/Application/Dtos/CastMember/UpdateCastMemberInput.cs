using Application.Messages;
using Domain.Enum;
using MediatR;

namespace Application.Dtos.CastMember;
public class UpdateCastMemberInput : IRequest<BaseResponse<CastMemberOutput>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }

    public UpdateCastMemberInput(Guid id, string name, CastMemberType type)
    {
        this.Id = id;
        Name = name;
        Type = type;
    }
}
