using Application.Dtos.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Category;
public class UpdateCategoryApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Api", "Category - Update")]
    public async Task Update()
    {
        var inputCreate = CreateCategoryInputGenerator.GetCategoryInput();

        var (responseCreate, outputCreate) = await apiClient
            .Post<CategoryOutput>(RESOURCE_URL, inputCreate);

        var inputUpdate = UpdateCategoryInputGenerator.GetCategory(outputCreate!.Id);

        var (response, output) = await apiClient
            .Put<CategoryOutput>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output!.Id.Should().Be(outputCreate.Id);
        output!.Name.Should().Be(inputUpdate.Name);
        output!.Description.Should().Be(inputUpdate.Description);
        output!.IsActive.Should().BeTrue();
        output!.CreatedAt.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(ErrorCategoryNotFound))]
    [Trait("Integration/Api", "Category - Update")]
    public async Task ErrorCategoryNotFound()
    {
        var inputUpdate = UpdateCategoryInputGenerator.GetCategory();

        var (response, output) = await apiClient
            .Put<ProblemDetails>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        output.Should().NotBeNull();
        output!.Title.Should().Be("An unexpected error ocurred");
        output!.Type.Should().Be("UnexpectedError");
        output!.Status.Should().Be((int) HttpStatusCode.InternalServerError);
        output!.Detail.Should().Be($"Category '{inputUpdate.Id}' not found.");
    }
}
