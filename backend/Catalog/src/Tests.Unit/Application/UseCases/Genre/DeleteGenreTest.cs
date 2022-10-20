using Application.Dtos.Genre;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Tests.Common.Generators.Entities;
using GenreUseCases = Application.UseCases.Genre;

namespace Tests.Unit.Application.UseCases.Genre;
public class DeleteGenreTest : GenreBaseFixture
{
    private readonly IDeleteGenre _deleteGenre;

    public DeleteGenreTest()
    {
        _deleteGenre = new GenreUseCases.DeleteGenre(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task DeleteGenre()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();
        var input = new DeleteGenreInput(exampleGenre.Id);

        _repositoryMock
            .Setup(r => r.Get(exampleGenre.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        _repositoryMock
            .Setup(r => r.Delete(exampleGenre, It.IsAny<CancellationToken>()));


        await _deleteGenre.Handle(input, CancellationToken.None);


        _repositoryMock.Verify(
            x => x.Get(
                exampleGenre.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        _repositoryMock.Verify(
            x => x.Delete(
                exampleGenre,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var exampleId = Guid.NewGuid();
        
        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleId),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre '{exampleId}' not found"
        ));
        
        var input = new DeleteGenreInput(exampleId);


        var action = async () => await _deleteGenre.Handle(input, CancellationToken.None);


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
