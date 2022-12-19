using Domain.Entity;
using Domain.Enum;

namespace Tests.Common.Generators.Entities;
public class CastMemberGenerator : CommonGenerator
{
    public static string GetCastMamemberName() => GetFaker().Person.FirstName;

    public static CastMemberType GetRandomCastMemberType() =>
        (CastMemberType) new Random().Next(1);

    public static CastMember GetFakerCastMember() => new(
        GetCastMamemberName(),
        GetRandomCastMemberType()
    );

    public static List<CastMember> GetExampleCastMembersList(int count = 5)
    {
        var list = new List<CastMember>();
        for (var i = 1; i < count; i++)
            list.Add(GetFakerCastMember());
        return list;
    }
}
