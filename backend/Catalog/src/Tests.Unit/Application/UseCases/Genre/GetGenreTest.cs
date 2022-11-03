using Application.Dtos.Genre;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Tests.Common.Generators.Entities;
using GenreUseCases = Application.UseCases.Genre;

namespace Tests.Unit.Application.UseCases.Genre;
public class GetGenreTest : GenreBaseFixture
{
    private readonly IGetGenre _getGenre;

    public GetGenreTest()
    {
        _getGenre = new GenreUseCases.GetGenre(_repositoryMock.Object);
    }

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task GetGenre()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre(
            categoriesIdsList: GenreGenerator.GetRandomIdsList(10)
        );

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var input = new GetGenreInput(exampleGenre.Id);

        
        var output = await _getGenre.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(exampleGenre.Name);
        output.Data.Is_Active.Should().Be(exampleGenre.IsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(exampleGenre.Categories.Count);
        
        foreach (var expectedId in exampleGenre.Categories)
            output.Data.Categories.Should().Contain(relation => relation.Id == expectedId);

        _repositoryMock.Verify(
            x => x.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var exampleId = Guid.NewGuid();

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleId),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre '{exampleId}' not found"
        ));
        var input = new GetGenreInput(exampleId);

        var action = async () => await _getGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found");

        _repositoryMock.Verify(
            x => x.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
