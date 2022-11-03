using Application.Exceptions;
using Application.Interfaces.UseCases;
using Application.UseCases.Genre;
using Tests.Common.Generators.Entities;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Application.UseCases.Genre;
public class CreateGenreTest : GenreTestFixture
{

    private readonly ICreateGenre _createGenre;

    public CreateGenreTest()
    {
        _createGenre = new CreateGenre(
            repository, unitOfWork, categoryRepository
        );
    }

    [Fact(DisplayName = nameof(CreateGenre))]
    [Trait("Integration/Application", "CreateGenre - Use Cases")]
    public async Task CreateGenre()
    {
        var input = CreateGenreInputGenerator.GetExampleInput();


        var output = await _createGenre.Handle(input, CancellationToken.None);


        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Is_Active.Should().Be(input.Is_Active);
        output.Data.Created_At.Should().NotBe(default);
        output.Data.Categories.Should().HaveCount(0);
    }
}
