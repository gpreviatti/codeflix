using Application.Dtos.Category;
using Application.Dtos.Genre;
using Application.Messages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.Genre;
public class ListGenresApiTest : GenreApiTestFixture
{
    [Fact(DisplayName = nameof(List))]
    [Trait("Integration/Api", "Genre - List")]
    public async Task List()
    {
        var inputs = GenreGenerator.GetExampleGenresList(10);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);
        });

        var page = 1;
        var perPage = 3;
        var parameters = new[] {
            KeyValuePair.Create("page", page.ToString()),
            KeyValuePair.Create("per_page", perPage.ToString()),
        };

        var route = QueryHelpers.AddQueryString(RESOURCE_URL, parameters!);


        var (response, output) = await apiClient
            .Get<BasePaginatedResponse<List<GenreOutput>>>(route);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<List<GenreOutput>>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
    }

    [Fact(DisplayName = nameof(ListWithPageParams))]
    [Trait("Integration/Api", "Genre - List")]
    public async Task ListWithPageParams()
    {
        var inputs = GenreGenerator.GetExampleGenresList(10);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);
        });

        var page = 1;
        var perPage = 5;
        var parameters = new[] {
            KeyValuePair.Create("page", page.ToString()),
            KeyValuePair.Create("per_page", perPage.ToString()),
        };

        var route = QueryHelpers.AddQueryString(RESOURCE_URL, parameters!);


        var (response, output) = await apiClient.Get<BasePaginatedResponse<List<GenreOutput>>>(route);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<List<GenreOutput>>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
        output!.Meta.Page.Should().Be(page);
        output!.Meta.Per_Page.Should().Be(perPage);
        output!.Data.Count.Should().Be(perPage);
    }

    [Fact(DisplayName = nameof(ListWithSearch))]
    [Trait("Integration/Api", "Genre - List")]
    public async Task ListWithSearch()
    {
        var inputs = GenreGenerator.GetExampleGenresList(5);

        inputs.ToList().ForEach(input =>
        {
            _ = apiClient.Post<BaseResponse<GenreOutput>>(RESOURCE_URL, input);
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


        var (response, output) = await apiClient.Get<BasePaginatedResponse<List<GenreOutput>>>(route);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BasePaginatedResponse<List<GenreOutput>>>().And.NotBeNull();
        output!.Data.Should().NotBeNull();
        output!.Meta.Page.Should().Be(page);
        output!.Meta.Per_Page.Should().Be(perPage);
        output!.Data.FirstOrDefault()!.Name.Should().Be(search);
    }
}
