using Domain.Enum;
using DomainEntity = Domain.Entity;

namespace Application.Dtos.CastMember;
public class CastMemberOutput
{
    public CastMemberOutput(
        Guid id,
        string name,
        CastMemberType type,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static CastMemberOutput FromCastMember(DomainEntity.CastMember castMember) => new(
        castMember.Id,
        castMember.Name,
        castMember.Type,
        castMember.CreatedAt
    );
}
