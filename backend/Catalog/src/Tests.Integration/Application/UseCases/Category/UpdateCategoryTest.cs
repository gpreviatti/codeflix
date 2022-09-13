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
    [Trait("Integration", "Update - Use Cases")]
    public async Task Update()
    {
        var category = CategoryGenerator.GetCategory();
        await _categoryRepository.Insert(category, CancellationToken.None);
        await _unitOfWork.Commit(CancellationToken.None);

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
}
