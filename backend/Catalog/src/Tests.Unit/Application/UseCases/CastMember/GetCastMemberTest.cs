using Application.Dtos.CastMember;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Repository;
using Tests.Common.Generators.Entities;
using UseCase = Application.UseCases.CastMember;

namespace Tests.Unit.Application.UseCases.CastMember;
public class GetCastMemberTest : CastMemberBaseFixture
{
    private readonly IGetCastMember _useCase;

    public GetCastMemberTest()
    {
        _useCase = new UseCase.GetCastMember(
            _repositoryMock.Object
        );
    }

    [Fact(DisplayName = nameof(GetCastMember))]
    [Trait("Application", "GetCastMember - Use Cases")]
    public async Task GetCastMember()
    {
        var repositoryMock = new Mock<ICastMemberRepository>();
        var castMemberExample = CastMemberGenerator.GetFakerCastMember();
        repositoryMock
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(castMemberExample);
        var input = new GetCastMemberInput(castMemberExample.Id);
        var useCase = new UseCase.GetCastMember(repositoryMock.Object);


        var output = await useCase.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Data.Id.Should().Be(castMemberExample.Id);
        output.Data.Name.Should().Be(castMemberExample.Name);
        output.Data.Type.Should().Be(castMemberExample.Type);
        repositoryMock.Verify(x => x.Get(
            It.Is<Guid>(x => x == input.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowIfNotFound))]
    [Trait("Application", "GetCastMember - Use Cases")]
    public async Task ThrowIfNotFound()
    {
        _repositoryMock
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("notfound"));
        var input = new GetCastMemberInput(Guid.NewGuid());


        var action = async () => await _useCase.Handle(input, CancellationToken.None);


        await action.Should().ThrowAsync<NotFoundException>();
    }
}
