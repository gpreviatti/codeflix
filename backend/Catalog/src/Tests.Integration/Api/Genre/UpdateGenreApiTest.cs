using Application.Dtos.Genre;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.Genre;
public class UpdateGenreApiTest : GenreApiTestFixture
{
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Api", "Category - Update")]
    public async Task Update()
    {
        var inputCreate = CreateGenreInputGenerator.GetExampleInput();

        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<GenreOutput>>(RESOURCE_URL, inputCreate);

        var inputUpdate = UpdateGenreInputGenerator.GetGenre(outputCreate!.Data.Id);

        var (response, output) = await apiClient
            .Put<BaseResponse<GenreOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<GenreOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
        output!.Data.Is_Active.Should().Be(inputUpdate.Is_Active.GetValueOrDefault());
        output!.Data.Created_At.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Integration/Api", "Genre - Update")]
    public async Task UpdateOnlyName()
    {
        var inputCreate = CreateGenreInputGenerator.GetExampleInput();
        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<GenreOutput>>(RESOURCE_URL, inputCreate);

        var inputUpdate = new UpdateGenreInput(
            outputCreate!.Data.Id, 
            CategoryGenerator.GetProductName()
        );

        var (response, output) = await apiClient
            .Put<BaseResponse<GenreOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<GenreOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
        output!.Data.Is_Active.Should().Be(outputCreate.Data.Is_Active);
        output!.Data.Created_At.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(ErrorGenreNotFound))]
    [Trait("Integration/Api", "Genre - Update")]
    public async Task ErrorGenreNotFound()
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
        output!.Detail.Should().Be($"Genre '{inputUpdate.Id}' not found.");
    }
}
