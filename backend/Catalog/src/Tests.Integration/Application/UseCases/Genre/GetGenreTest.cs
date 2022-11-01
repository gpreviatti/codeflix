using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Application.UseCases.Genre;
using Infra.Data.Repositories;
using Tests.Common.Generators.Dtos;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Genre;
public class GetGenreTest : GenreTestFixture
{
    private readonly IGetGenre _getGenre;

    public GetGenreTest()
    {
        _getGenre = new GetGenre(repository);
    }

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Integration/Application", "GetGenre - Use Cases")]
    public async Task GetGenre()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var trackingInfo = await dbContext.AddAsync(genre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Fix tracking problem in ef core
        trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        var input = new GetGenreInput(genre.Id);


        var output = await _getGenre.Handle(input, CancellationToken.None);


        output.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(genre.Id);
        output.Data.Name.Should().Be(genre.Name);
        output.Data.IsActive.Should().Be(genre.IsActive);
    }
}
