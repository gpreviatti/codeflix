using Application.Dtos.Category;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Category;
public class CreateCategoryApiTest : CategoryApiTestFixture
{
    [Fact]
    [Trait("Integration/Api", "Category - Create")]
    public async Task Create()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var (response, output) = await apiClient.Post<CategoryOutput>(RESOURCE_URL, input);
        var category = await GetById(output!.Id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output!.Id.Should().NotBeEmpty();
        output!.Name.Should().Be(input.Name);
        output!.Description.Should().Be(input.Description);
        output!.IsActive.Should().BeTrue();
        output!.CreatedAt.Should().NotBe(default);

        category.Should().NotBeNull();
        category!.Name.Should().Be(output!.Name);
    }
}
