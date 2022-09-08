using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using FluentAssertions;

namespace Tests.Integration.Application.UseCases.Category;
public class GetCategoryTest : CategoryTestFixture
{
    private readonly IGetCategory _getCategory;

    public GetCategoryTest()
    {
        _getCategory = new GetCategory(_categoryRepository);
    }

    [Fact]
    [Trait("Integration", "GetCategoryTest - Use Cases")]
    public async Task GetCategory()
    {
        var category = GetValidCategory();
        await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var input = new GetCategoryInput(category.Id);

        var output = await _getCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().Be(category.Id);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().NotBe(default);
    }

    [Fact]
    [Trait("Integration", "NotFoundExceptionWhenCategoryDoesntExist - Use Cases")]
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
