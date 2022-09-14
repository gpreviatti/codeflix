using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Application.UseCases.Category;

public class ListCategoryTest : CategoryTestFixture
{
    private readonly IListCategories _listCategories;

    public ListCategoryTest()
    {
        _listCategories = new ListCategories(_categoryRepository);
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

        output.GetType().Should().Be<ListCategoriesOutput>().And.NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Filtred.Should().Be(count);
        output.Total.Should().Be(count);
        output.Items.Count.Should().Be(count);
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

        output.GetType().Should().Be<ListCategoriesOutput>().And.NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Filtred.Should().Be(categoriesFiltred.Count());
        output.Items.Count.Should().Be(categoriesFiltred.Count());
        output.Total.Should().Be(categories.Count);
    }
}
