using Application.Dtos.Genre;
using Application.Messages;
using System.Net;
using Tests.Common.Generators.Dtos;
using Tests.Integration.Api.Category;

namespace Tests.Integration.Api.Genre;
public class CreateGenreApiTest : GenreApiTestFixture
{
    [Fact(DisplayName = nameof(Create))]
    [Trait("Integration/Api", "Genre - Create")]
    public async Task Create()
    {
        var input = CreateGenreInputGenerator.GetExampleInput();

        var (response, output) = await apiClient
            .Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<GenreOutput>>().And.NotBeNull();
        output!.Data.Id.Should().NotBeEmpty();
        output!.Data.Name.Should().Be(input.Name);
        output!.Data.Is_Active.Should().Be(input.Is_Active);
    }
}
