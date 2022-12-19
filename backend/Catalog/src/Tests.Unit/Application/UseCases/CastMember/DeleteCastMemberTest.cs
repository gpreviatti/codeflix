using Application.Dtos.CastMember;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using UseCase = Application.UseCases.CastMember;

namespace Tests.Unit.Application.UseCases.CastMember;
public class DeleteCastMemberTest : CastMemberBaseFixture
{
    private readonly IDeleteCastMember _useCase;

    public DeleteCastMemberTest()
    {
        _useCase = new UseCase.DeleteCastMember(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact(DisplayName = nameof(DeleteCastMember))]
    [Trait("Application", "DeleteCastMemberWithSuccess - Use Cases")]
    public async Task DeleteCastMember()
    {
        var castMemberExample = CastMemberGenerator.GetFakerCastMember();
        _repositoryMock
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(castMemberExample);
        var input = new DeleteCastMemberInput(castMemberExample.Id);

        var action = async () => await _useCase.Handle(input, CancellationToken.None);

        await action.Should().NotThrowAsync();
        _repositoryMock.Verify(
            x => x.Get(It.Is<Guid>(x => x == input.Id), It.IsAny<CancellationToken>()),
            Times.Once
        );
        _repositoryMock.Verify(
            x => x.Delete(
                It.Is<DomainEntity.CastMember>(x => x.Id == input.Id),
                It.IsAny<CancellationToken>()),
            Times.Once
        );
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>())
            , Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowsWhenNotFound))]
    [Trait("Application", "DeleteCastMember - Use Cases")]
    public async Task ThrowsWhenNotFound()
    {
        _repositoryMock
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("notFound"));
        
        var input = new DeleteCastMemberInput(Guid.NewGuid());

        var action = async () => await _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }
}
