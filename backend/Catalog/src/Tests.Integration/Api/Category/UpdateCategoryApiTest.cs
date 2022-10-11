using Application.Dtos.Category;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.Category;
public class UpdateCategoryApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Api", "Category - Update")]
    public async Task Update()
    {
        var inputCreate = CreateCategoryInputGenerator.GetCategoryInput();

        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, inputCreate);

        var inputUpdate = UpdateCategoryInputGenerator.GetCategory(outputCreate!.Data.Id);

        var (response, output) = await apiClient
            .Put<BaseResponse<CategoryOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CategoryOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
        output!.Data.Description.Should().Be(inputUpdate.Description);
        output!.Data.Is_Active.Should().BeTrue();
        output!.Data.Created_At.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Integration/Api", "Category - Update")]
    public async Task UpdateOnlyName()
    {
        var inputCreate = CreateCategoryInputGenerator.GetCategoryInput();
        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, inputCreate);

        var inputUpdate = new UpdateCategoryInput(
            outputCreate!.Data.Id, 
            CategoryGenerator.GetProductName()
        );

        var (response, output) = await apiClient
            .Put<BaseResponse<CategoryOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CategoryOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
        output!.Data.Description.Should().Be(outputCreate.Data.Description);
        output!.Data.Is_Active.Should().Be(outputCreate.Data.Is_Active);
        output!.Data.Created_At.Should().NotBe(default);
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
