using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Application.Messages;
using Application.UseCases.Genre;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Genre;

public class ListGenresTest : GenreTestFixture
{
    private readonly IListGenres _useCase;

    public ListGenresTest()
    {
        _useCase = new ListGenres(repository, categoryRepository);
    }

    [Fact]
    [Trait("Integration/Application", "List - Use Cases")]
    public async Task List()
    {
        var count = 10;
        var genres = GenreGenerator.GetExampleGenresList(count);
        await dbContext.AddRangeAsync(genres, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var input = new ListGenresInput(1, 10);


        var output = await _useCase.Handle(input, CancellationToken.None);


        output.GetType().Should().Be<BasePaginatedResponse<List<GenreOutput>>>().And.NotBeNull();
        output.Meta.Page.Should().Be(input.Page);
        output.Meta.Per_Page.Should().Be(input.Per_Page);
        output.Meta.Filtred.Should().Be(output.Meta.Total);
        output.Data.Count.Should().Be(count);
    }

    [Fact]
    [Trait("Integration/Application", "ListWithSearchTerm - Use Cases")]
    public async Task ListWithSearchTerm()
    {
        var count = 10;
        var genres = GenreGenerator.GetExampleGenresList(count);
        await dbContext.AddRangeAsync(genres, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var search = genres.FirstOrDefault()?.Name;
        var filtred = dbContext.Genres.Where(c => c.Name.Equals(search)).ToList();
        var total = dbContext.Genres.Count();

        var input = new ListGenresInput(1, 10, search!);


        var output = await _useCase.Handle(input, CancellationToken.None);


        output.GetType().Should().Be<BasePaginatedResponse<List<GenreOutput>>>().And.NotBeNull();
        output.Data.Count.Should().Be(input.Per_Page);
        output.Meta.Page.Should().Be(input.Page);
        output.Meta.Per_Page.Should().Be(input.Per_Page);
        output.Meta.Filtred.Should().Be(filtred.Count);
        output.Meta.Total.Should().Be(total);
    }
}
