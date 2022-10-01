﻿using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Category;

public class GetCategoryTest : CategoryTestFixture
{
    private readonly IGetCategory _getCategory;

    public GetCategoryTest()
    {
        _getCategory = new GetCategory(_categoryRepository);
    }

    [Fact]
    [Trait("Integration/Application", "GetCategoryTest - Use Cases")]
    public async Task GetCategory()
    {
        var category = CategoryGenerator.GetCategory();
        await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var input = new GetCategoryInput(category.Id);

        var output = await _getCategory.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Description.Should().Be(category.Description);
        output.Is_Active.Should().Be(category.IsActive);
        output.Created_At.Should().NotBe(default);
    }

    [Fact]
    [Trait("Integration/Application", "NotFoundExceptionWhenCategoryDoesntExist - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var guid = Guid.NewGuid();
        var input = new GetCategoryInput(guid);

        var task = async () => await _getCategory.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NullReferenceException>()
            .WithMessage($"Category '{guid}' not found.");
    }
}
