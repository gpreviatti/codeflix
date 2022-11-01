using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Application.UseCases.Category;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Category;

public class ListCategoryTest : CategoryTestFixture
{
    private readonly IListCategories _listCategories;

    public ListCategoryTest()
    {
        _listCategories = new ListCategories(categoryRepository);
    }

    [Fact]
    [Trait("Integration/Application", "List - Use Cases")]
    public async Task List()
    {
        var count = 10;
        var categories = CategoryGenerator.GetCategories(count);
        await dbContext.AddRangeAsync(categories, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var input = new ListCategoriesInput(1, 10);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>().And.NotBeNull();
        output.Meta.Page.Should().Be(input.Page);
        output.Meta.Per_Page.Should().Be(input.Per_Page);
        output.Meta.Filtred.Should().Be(output.Meta.Total);
        output.Data.Count.Should().Be(count);
    }

    [Fact]
    [Trait("Integration/Application", "ListWithSearchTerm - Use Cases")]
    public async Task ListWithSearchTerm()
    {
        var count = 10;
        var categories = CategoryGenerator.GetCategories(count);
        await dbContext.AddRangeAsync(categories, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var search = categories.FirstOrDefault()?.Name;
        var categoriesFiltred = categories.Where(c => c.Name.Equals(search));

        var input = new ListCategoriesInput(1, 10, search!);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>().And.NotBeNull();
        output.Meta.Page.Should().Be(input.Page);
        output.Meta.Per_Page.Should().Be(input.Per_Page);
        output.Meta.Filtred.Should().Be(categoriesFiltred.Count());
        output.Data.Count.Should().Be(categoriesFiltred.Count());
    }
}
