using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Domain.Excpetions;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using UseCase = Application.UseCases.CastMember;

namespace Tests.Unit.Application.UseCases.CastMember;
public class CreateCastMemberTest : CastMemberBaseFixture
{
    private readonly ICreateCastMember _useCase;
    public CreateCastMemberTest()
    {
        _useCase = new UseCase.CreateCastMember(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "CreateCastMember - Use Cases")]
    public async Task Create()
    {
        var input = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );       

        var output = await _useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Type.Should().Be(input.Type);
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        _repositoryMock.Verify(
            x => x.Insert(
                It.Is<DomainEntity.CastMember>(
                    x => (x.Name == input.Name && x.Type == input.Type)
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowsWhenInvalidName))]
    [Trait("Application", "CreateCastMember - Use Cases")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task ThrowsWhenInvalidName(string? name)
    {
        var input = new CreateCastMemberInput(
            name!,
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var action = async () => await _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
}
