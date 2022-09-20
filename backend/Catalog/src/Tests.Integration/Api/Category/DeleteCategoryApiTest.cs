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

        var (responseCreate, outputCreate) = await apiClient
            .Post<CategoryOutput>(RESOURCE_URL, input);


        var (responseDelete, outputDelete) = await apiClient
            .Delete<CategoryOutput>(RESOURCE_URL + "/" + outputCreate!.Id);

        var (responseGet, outputGet) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + outputCreate!.Id);


        responseDelete!.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGet!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        outputGet!.Detail.Should().Be($"Category '{outputCreate!.Id}' not found.");

    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Integration/Api", "Category - Delete")]
    public async Task ThrowWhenCategoryNotFound()
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
