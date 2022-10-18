using Application.Dtos.Category;
using Application.Messages;
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
            .Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, input);

        
        var responseDelete = await apiClient.Delete(RESOURCE_URL + "/" + outputCreate!.Data.Id);

        var (responseGet, outputGet) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + outputCreate!.Data.Id);


        responseDelete!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        outputGet!.Detail.Should().Be($"Category '{outputCreate!.Data.Id}' not found.");
    }

    [Fact(DisplayName = nameof(ErrorCategoryNotFound))]
    [Trait("Integration/Api", "Category - Delete")]
    public async Task ErrorCategoryNotFound()
    {
        var id = Guid.NewGuid();
        
        
        var response = await apiClient.Delete(RESOURCE_URL + "/" + id);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
