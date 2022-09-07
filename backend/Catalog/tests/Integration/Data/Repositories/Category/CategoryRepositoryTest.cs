using Domain.Repository;
using Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Infra.Data.Repositories;

namespace Integration.Data.Repositories.Category;
public class CategoryRepositoryTest : CategoryRepositoryTestFixture
{
    private ICategoryRepository repoistory;

    public CategoryRepositoryTest()
    {
        repoistory = new CategoryRepository(dbContext);
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Data", "Category - Repositories")]
    public async Task Insert()
    {
        var category = GetCategory();

        await repoistory.Insert(category, CancellationToken.None);
        await SaveChanges();

        var dbCategory = await dbContext.Categories.FindAsync(category.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(category.Name);
        dbCategory!.Description.Should().Be(category.Description);
        dbCategory!.IsActive.Should().Be(category.IsActive);
        dbCategory!.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Data", "Category - Repositories")]
    public async Task Get()
    {
        var category = GetCategory();
        await dbContext.AddAsync(category);
        await SaveChanges();

        var result = await repoistory.Get(category.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        result.IsActive.Should().Be(category.IsActive);
        result.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Data", "Category - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        var exampleId = Guid.NewGuid();

        var task = async () => await repoistory
            .Get(exampleId, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NullReferenceException>()
            .WithMessage($"Category '{exampleId}' not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Data", "Category - Repositories")]
    public async Task Update()
    {
        var category = GetCategory();
        await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var newName = Faker.Commerce.ProductName();
        category.Update(newName);
        category.Deactivate();
        dbContext = CreateDbContext();
        repoistory = new CategoryRepository(dbContext);

        await repoistory.Update(category, CancellationToken.None);
        await SaveChanges();
        var dbCategory = await dbContext.Categories.FindAsync(category.Id);

        dbCategory!.IsActive.Should().BeFalse();
        dbCategory.Name.Should().Be(newName);
        dbCategory.CreatedAt.Should().Be(category.CreatedAt);
        dbCategory.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Data", "Category - Repositories")]
    public async Task Delete()
    {
        var category = GetCategory();
        await dbContext.AddAsync(category);
        await SaveChanges();

        await repoistory.Delete(category, CancellationToken.None);
        await SaveChanges();
        var result = await CreateDbContext().Categories.FindAsync(category.Id);

        result.Should().BeNull();
    }

    #region Search
    [Theory(DisplayName = nameof(Search))]
    [Trait("Data", "Category - Repositories")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    [InlineData("id", SearchOrder.Asc)]
    [InlineData("id", SearchOrder.Desc)]
    [InlineData("createdat", SearchOrder.Asc)]
    [InlineData("createdat", SearchOrder.Desc)]
    [InlineData("", SearchOrder.Desc)]
    public async Task Search(string order, SearchOrder searchOrder)
    {
        var categories = GetCategories(15);
        await dbContext.AddRangeAsync(categories);
        await SaveChanges();

        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, "", order, searchOrder);

        var result = await repoistory.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Items.Count.Should().Be(perPage);
    }

    [Fact(DisplayName = nameof(SearchWithSearchTerm))]
    [Trait("Data", "Category - Repositories")]
    public async Task SearchWithSearchTerm()
    {
        var totalItens = 15;
        var categories = GetCategories(totalItens);
        await dbContext.AddRangeAsync(categories);
        await SaveChanges();

        var searchTerm = categories.FirstOrDefault()!.Name;
        var recordsFiltred = categories
            .Where(c => c.Name.Equals(searchTerm)).Count();

        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, searchTerm, "name", SearchOrder.Desc);

        var result = await repoistory.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Items.Count.Should().Be(recordsFiltred);
        result.Total.Should().Be(totalItens);
    }

    [Fact(DisplayName = nameof(SearcReturnsEmpty))]
    [Trait("Data", "Category - Repositories")]
    public async Task SearcReturnsEmpty()
    {
        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, "", "", SearchOrder.Desc);

        var result = await repoistory.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Items.Count.Should().Be(0);
        result.Total.Should().Be(0);
    }
    #endregion
}
