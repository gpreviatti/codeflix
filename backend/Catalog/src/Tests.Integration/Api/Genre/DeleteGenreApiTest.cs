using Application.Dtos.Genre;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Genre;
public class DeleteGenreApiTest : GenreApiTestFixture
{
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Api", "Genre - Delete")]
    public async Task Delete()
    {
        var input = CreateGenreInputGenerator.GetExampleInput();
        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);

        
        var responseDelete = await apiClient.Delete(RESOURCE_URL + "/" + outputCreate!.Data.Id);

        var (responseGet, outputGet) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + outputCreate!.Data.Id);


        responseDelete!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        outputGet!.Detail.Should().Be($"Genre '{outputCreate!.Data.Id}' not found.");
    }

    [Fact(DisplayName = nameof(ErrorGenreNotFound))]
    [Trait("Integration/Api", "Genre - Delete")]
    public async Task ErrorGenreNotFound()
    {
        var id = Guid.NewGuid();
        
        
        var response = await apiClient.Delete(RESOURCE_URL + "/" + id);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
