using Application.Dtos.Category;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.Category;
public class ListCategoriesApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(List))]
    [Trait("Integration/Api", "Category - List")]
    public async Task List()
    {
        var inputs = CategoryGenerator.GetCategories(5);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<CategoryOutput>(RESOURCE_URL, input);
        });

        var (response, output) = await apiClient
            .Get<ListCategoriesOutput>(RESOURCE_URL + "/");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<ListCategoriesOutput>().And.NotBeNull();
        output!.Items.Should().NotBeNull();
    }

    [Fact(DisplayName = nameof(ListWithPageParams))]
    [Trait("Integration/Api", "Category - List")]
    public async Task ListWithPageParams()
    {
        var inputs = CategoryGenerator.GetCategories(10);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<CategoryOutput>(RESOURCE_URL, input);
        });
        var page = 1;
        var perPage = 10;

        var (response, output) = await apiClient
            .Get<ListCategoriesOutput>(RESOURCE_URL + $"?page={page}&perPage={perPage}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<ListCategoriesOutput>().And.NotBeNull();
        output!.Items.Should().NotBeNull();
        output!.Page.Should().Be(page);
        output!.PerPage.Should().Be(perPage);
        output!.Items.Count.Should().Be(perPage);
    }

    [Fact(DisplayName = nameof(ListWithSearch))]
    [Trait("Integration/Api", "Category - List")]
    public async Task ListWithSearch()
    {
        var inputs = CategoryGenerator.GetCategories(5);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<CategoryOutput>(RESOURCE_URL, input);
        });
        var page = 1;
        var perPage = 10;
        var search = inputs.FirstOrDefault()!.Name;

        var (response, output) = await apiClient
            .Get<ListCategoriesOutput>(RESOURCE_URL + $"?page={page}&perPage={perPage}&search={search}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<ListCategoriesOutput>().And.NotBeNull();
        output!.Items.Should().NotBeNull();
        output!.Page.Should().Be(page);
        output!.PerPage.Should().Be(perPage);
        output!.Items.Count.Should().Be(1);
        output!.Items.FirstOrDefault()!.Name.Should().Be(search);
    }
}
