using Application.Dtos.Category;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Category;
public class CreateCategoryApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(Create))]
    [Trait("Integration/Api", "Category - Create")]
    public async Task Create()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var (response, output) = await apiClient
            .Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CategoryOutput>>().And.NotBeNull();
        output!.Data.Id.Should().NotBeEmpty();
        output!.Data.Name.Should().Be(input.Name);
        output!.Data.Description.Should().Be(input.Description);
        output!.Data.Is_Active.Should().BeTrue();
        output!.Data.Created_At.Should().NotBe(default);
    }

    [Theory(DisplayName = nameof(ErrorCantInstantiateAggregate))]
    [Trait("Integration/Api", "Category - Create")]
    [MemberData(
        nameof(CreateCategoryInputGenerator.GetE2eInvalidInputs),
        MemberType = typeof(CreateCategoryInputGenerator)
    )]
    public async Task ErrorCantInstantiateAggregate(
        string expectedDetail,
        CreateCategoryInput input
    )
    {
        var (response, output) = await apiClient
            .Post<ProblemDetails>(RESOURCE_URL, input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output!.Type.Should().Be("UnprocessableEntity");
        output!.Status.Should().Be((int) HttpStatusCode.UnprocessableEntity);
        output!.Detail.Should().Be(expectedDetail);
    }
}
