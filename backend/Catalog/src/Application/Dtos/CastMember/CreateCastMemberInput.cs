using Application.Messages;
using Domain.Enum;
using MediatR;

namespace Application.Dtos.CastMember;
public class CreateCastMemberInput : IRequest<BaseResponse<CastMemberOutput>>
{
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }

    public CreateCastMemberInput(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }
}