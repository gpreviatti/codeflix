using Application.Dtos.Genre;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Genre;
public class GetGenreApiTest : GenreApiTestFixture
{
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Api", "Genre - Get")]
    public async Task Get()
    {
        var input = CreateGenreInputGenerator.GetExampleInput();
        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);

        var (response, output) = await apiClient
            .Get<BaseResponse<GenreOutput>>(RESOURCE_URL + "/" + outputCreate!.Data.Id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<GenreOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate.Data.Id);
        output!.Data.Name.Should().Be(input.Name);
        output!.Data.Is_Active.Should().Be(input.Is_Active);
        output!.Data.Created_At.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(ErrorGenreNotFound))]
    [Trait("Integration/Api", "Genre - Get")]
    public async Task ErrorGenreNotFound()
    {
        var id = Guid.NewGuid();
        var (response, output) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        output.Should().NotBeNull();
        output!.Title.Should().Be("An unexpected error ocurred");
        output!.Type.Should().Be("UnexpectedError");
        output!.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        output!.Detail.Should().Be($"Genre '{id}' not found.");
    }
}
