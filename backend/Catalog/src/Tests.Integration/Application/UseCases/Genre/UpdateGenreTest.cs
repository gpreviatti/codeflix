using Application.Exceptions;
using Application.Interfaces.UseCases;
using Application.UseCases.Genre;
using Tests.Common.Generators.Entities;
using Tests.Common.Generators.Dtos;
using Application.Dtos.Genre;

namespace Tests.Integration.Application.UseCases.Genre;
public class UpdateGenreTest : GenreTestFixture
{

    private readonly IUpdateGenre _updateGenre;

    public UpdateGenreTest()
    {
        _updateGenre = new UpdateGenre(
            repository, unitOfWork, categoryRepository
        );
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Integration/Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenre()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var trackingInfo = await dbContext.AddAsync(genre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Fix tracking problem in ef core
        trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        var input = new UpdateGenreInput(genre.Id, genre.Name + " Updated");


        var output = await _updateGenre.Handle(input, CancellationToken.None);


        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.IsActive.Should().Be(genre.IsActive);
    }
}
