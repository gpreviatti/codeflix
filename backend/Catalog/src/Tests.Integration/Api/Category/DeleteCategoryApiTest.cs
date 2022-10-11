using Application.Dtos.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Category;
public class DeleteCategoryApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Api", "Category - Delete")]
    public async Task Delete()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();
        var (_, outputCreate) = await apiClient
            .Post<CategoryOutput>(RESOURCE_URL, input);

        var (responseDelete, _) = await apiClient
            .Delete<object>(RESOURCE_URL + "/" + outputCreate!.Id);

        var (responseGet, outputGet) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + outputCreate!.Id);


        responseDelete!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        outputGet!.Detail.Should().Be($"Category '{outputCreate!.Id}' not found.");

    }

    [Fact(DisplayName = nameof(ErrorCategoryNotFound))]
    [Trait("Integration/Api", "Category - Delete")]
    public async Task ErrorCategoryNotFound()
    {
        var id = Guid.NewGuid();
        
        
        var (response, output) = await apiClient
            .Delete<ProblemDetails>(RESOURCE_URL + "/" + id);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        output.Should().NotBeNull();
        output!.Title.Should().Be("An unexpected error ocurred");
        output!.Type.Should().Be("UnexpectedError");
        output!.Status.Should().Be((int) HttpStatusCode.InternalServerError);
        output!.Detail.Should().Be($"Category '{id}' not found.");
    }
}
