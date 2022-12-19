using Domain.Entity;
using Domain.Excpetions;
using Tests.Common.Generators.Entities;

namespace Tests.Unit.Domain.Entity;
public class CastMemberTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Instantiate()
    {
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var name = CastMemberGenerator.GetCastMamemberName();
        var type = CastMemberGenerator.GetRandomCastMemberType();

        var castMember = new CastMember(name, type);

        var datetimeAfter = DateTime.Now.AddSeconds(1);
        castMember.Id.Should().NotBe(default(Guid));
        castMember.Name.Should().Be(name);
        castMember.Type.Should().Be(type);
        (castMember.CreatedAt >= datetimeBefore).Should().BeTrue();
        (castMember.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(ThrowErrorWhenNameIsInvalid))]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ThrowErrorWhenNameIsInvalid(string? name)
    {
        var type = CastMemberGenerator.GetRandomCastMemberType();

        var action = () => new CastMember(name!, type);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Update()
    {
        var newName = CastMemberGenerator.GetCastMamemberName();
        var newType = CastMemberGenerator.GetRandomCastMemberType();
        var castMember = CastMemberGenerator.GetFakerCastMember();

        castMember.Update(newName, newType);

        castMember.Name.Should().Be(newName);
        castMember.Type.Should().Be(newType);
    }

    [Theory(DisplayName = nameof(UpdateThrowsErrorWhenNameIsInvalid))]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateThrowsErrorWhenNameIsInvalid(string? newName)
    {
        var newType = CastMemberGenerator.GetRandomCastMemberType();
        var castMember = CastMemberGenerator.GetFakerCastMember();

        var action = () => castMember.Update(newName!, newType);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"Name should not be empty or null");
    }
}
