﻿using Application.Dtos.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Api.Category;
public class GetCategoryApiTest : CategoryApiTestFixture
{
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Api", "Category - Get")]
    public async Task Get()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();
        var (_, outputCreate) = await apiClient
            .Post<CategoryOutput>(RESOURCE_URL, input);

        var (response, output) = await apiClient
            .Get<CategoryOutput>(RESOURCE_URL + "/" + outputCreate!.Id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output!.Id.Should().Be(outputCreate.Id);
        output!.Name.Should().Be(input.Name);
        output!.Description.Should().Be(input.Description);
        output!.Is_Active.Should().BeTrue();
        output!.Created_At.Should().NotBe(default);
    }

    [Fact(DisplayName = nameof(ErrorCategoryNotFound))]
    [Trait("Integration/Api", "Category - Get")]
    public async Task ErrorCategoryNotFound()
    {
        var id = Guid.NewGuid();
        var (response, output) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        output.Should().NotBeNull();
        output!.Title.Should().Be("An unexpected error ocurred");
        output!.Type.Should().Be("UnexpectedError");
        output!.Status.Should().Be((int) HttpStatusCode.InternalServerError);
        output!.Detail.Should().Be($"Category '{id}' not found.");
    }
}
