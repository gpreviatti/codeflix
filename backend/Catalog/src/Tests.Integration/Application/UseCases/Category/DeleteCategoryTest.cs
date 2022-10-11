using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Category;

public class DeleteCategoryTest : CategoryTestFixture
{
    private readonly IDeleteCategory _deleteCategory;

    public DeleteCategoryTest()
    {
        _deleteCategory = new DeleteCategory(_categoryRepository, _unitOfWork);
    }

    [Fact]
    [Trait("Integration/Application", "Delete - Use Cases")]
    public async Task Delete()
    {
        var category = CategoryGenerator.GetCategory();
        var trackingInfo = await dbContext.AddAsync(category, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        trackingInfo.State = EntityState.Detached;

        var input = new DeleteCategoryInput(category.Id);

        await _deleteCategory.Handle(input, CancellationToken.None);
        var deletedCategory = await dbContext
            .Categories
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        deletedCategory.Should().BeNull();
    }

    [Fact]
    [Trait("Integration/Application", "DeleteThrowsWhenNotFoundCategory - Use Cases")]
    public async Task DeleteThrowsWhenNotFoundCategory()
    {
        var category = CategoryGenerator.GetCategory();

        var input = new DeleteCategoryInput(category.Id);

        var action = async () => await _deleteCategory
            .Handle(input, CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NullReferenceException>()
            .WithMessage($"Category '{category.Id}' not found.");
    }
}
