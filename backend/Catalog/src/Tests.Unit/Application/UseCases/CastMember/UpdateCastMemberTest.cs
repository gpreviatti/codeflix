using Application.Dtos.CastMember;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Excpetions;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using UseCase = Application.UseCases.CastMember;


namespace Tests.Unit.Application.UseCases.CastMember;
public class UpdateCastMemberTest : CastMemberBaseFixture
{
    private IUpdateCastMember _useCase;

    public UpdateCastMemberTest()
    {
        _useCase = new UseCase.UpdateCastMember(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task Update()
    {
        var castMemberExample = CastMemberGenerator.GetFakerCastMember();
        var newName = CastMemberGenerator.GetCastMamemberName();
        var newType = CastMemberGenerator.GetRandomCastMemberType();
        _repositoryMock
            .Setup(x => x.Get(
                It.Is<Guid>(x => x == castMemberExample.Id),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(castMemberExample);

        var input = new UpdateCastMemberInput(
            castMemberExample.Id,
            newName,
            newType
        );

        var output = await _useCase.Handle(input, CancellationToken.None);

        output.Data.Id.Should().Be(castMemberExample.Id);
        output.Data.Name.Should().Be(newName);
        output.Data.Type.Should().Be(newType);

        _repositoryMock
            .Verify(x => x.Get(
                It.Is<Guid>(x => x == input.Id),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        _repositoryMock
            .Verify(x => x.Update(
                It.Is<DomainEntity.CastMember>(
                    x => (
                        x.Id == castMemberExample.Id &&
                        x.Name == input.Name &&
                        x.Type == input.Type
                    )
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>())
            , Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task ThrowWhenNotFound()
    {
        _repositoryMock
            .Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new NotFoundException("error"));
        var input = new UpdateCastMemberInput(
            Guid.NewGuid(),
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var action = async () => await _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }


    [Fact(DisplayName = nameof(ThrowWhenInvalidName))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task ThrowWhenInvalidName()
    {
        var castMemberExample = CastMemberGenerator.GetFakerCastMember();
        _repositoryMock
            .Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(castMemberExample);

        var input = new UpdateCastMemberInput(
            Guid.NewGuid(),
            null!,
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var action = async () => await _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
}
