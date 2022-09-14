using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Category;

public class UpdateCategoryTest : CategoryTestFixture
{
    private readonly IUpdateCategory _updateCategory;

    public UpdateCategoryTest()
    {
        _updateCategory = new UpdateCategory(_categoryRepository, _unitOfWork);
    }

    [Fact]
    [Trait("Integration/Application", "Update - Use Cases")]
    public async Task Update()
    {
        var category = CategoryGenerator.GetCategory();
        var trackingInfo = await dbContext.AddAsync(category, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        // Fix tracking problem in ef core
        trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        var newName = Faker.Commerce.ProductName();
        var input = new UpdateCategoryInput(category.Id, newName);

        var output = await _updateCategory.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(newName);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().NotBe(default);
    }

    [Fact]
    [Trait("Integration/Application", "UpdateThrowsWhenNotFoundCategory - Use Cases")]
    public async Task UpdateThrowsWhenNotFoundCategory()
    {
        var category = CategoryGenerator.GetCategory();

        var newName = Faker.Commerce.ProductName();
        var input = new UpdateCategoryInput(category.Id, newName);

        var action = async () => await _updateCategory
            .Handle(input, CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NullReferenceException>()
            .WithMessage($"Category '{category.Id}' not found.");
    }
}
