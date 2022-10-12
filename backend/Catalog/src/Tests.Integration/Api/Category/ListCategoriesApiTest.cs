using Application.Dtos.Category;
using Application.Messages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.Category;
public class ListCategoriesApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(List))]
    [Trait("Integration/Api", "Category - List")]
    public async Task List()
    {
        var inputs = CategoryGenerator.GetCategories(1);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, input);
        });

        var page = 1;
        var perPage = 3;
        var parameters = new[] {
            KeyValuePair.Create("page", page.ToString()),
            KeyValuePair.Create("per_page", perPage.ToString()),
        };

        var route = QueryHelpers.AddQueryString(RESOURCE_URL, parameters!);


        var (response, output) = await apiClient
            .Get<BasePaginatedResponse<List<CategoryOutput>>>(route);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
    }

    [Fact(DisplayName = nameof(ListWithPageParams))]
    [Trait("Integration/Api", "Category - List")]
    public async Task ListWithPageParams()
    {
        var inputs = CategoryGenerator.GetCategories(10);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, input);
        });

        var page = 1;
        var perPage = 5;
        var parameters = new[] {
            KeyValuePair.Create("page", page.ToString()),
            KeyValuePair.Create("per_page", perPage.ToString()),
        };

        var route = QueryHelpers.AddQueryString(RESOURCE_URL, parameters!);


        var (response, output) = await apiClient.Get<BasePaginatedResponse<List<CategoryOutput>>>(route);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
        output!.Meta.Page.Should().Be(page);
        output!.Meta.Per_Page.Should().Be(perPage);
        output!.Data.Count.Should().Be(perPage);
    }

    [Fact(DisplayName = nameof(ListWithSearch))]
    [Trait("Integration/Api", "Category - List")]
    public async Task ListWithSearch()
    {
        var inputs = CategoryGenerator.GetCategories(5);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<CategoryOutput>>(RESOURCE_URL, input);
        });
        var page = 1;
        var perPage = 10;
        var search = inputs.FirstOrDefault()!.Name;
        var parameters = new[] {
            KeyValuePair.Create("page", page.ToString()),
            KeyValuePair.Create("per_page", perPage.ToString()),
            KeyValuePair.Create("search", search),
        };

        var route = QueryHelpers.AddQueryString(RESOURCE_URL, parameters!);


        var (response, output) = await apiClient.Get<BasePaginatedResponse<List<CategoryOutput>>>(route);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<CategoryOutput>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
        output!.Meta.Page.Should().Be(page);
        output!.Meta.Per_Page.Should().Be(perPage);
        output!.Data.Count.Should().Be(1);
        output!.Data.FirstOrDefault()!.Name.Should().Be(search);
    }
}
