using Domain.Repository;
using Domain.SeedWork.SearchableRepository;
using Infra.Data.Repositories;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Data.Repositories;
public class CategoryRepositoryTest : BaseFixture
{
    private ICategoryRepository repository;

    public CategoryRepositoryTest()
    {
        repository = new CategoryRepository(dbContext);
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task Insert()
    {
        var category = CategoryGenerator.GetCategory();

        await repository.Insert(category, CancellationToken.None);
        await SaveChanges();

        var dbCategory = await dbContext.Categories.FindAsync(category.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(category.Name);
        dbCategory!.Description.Should().Be(category.Description);
        dbCategory!.IsActive.Should().Be(category.IsActive);
        dbCategory!.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task Get()
    {
        var category = CategoryGenerator.GetCategory();
        await dbContext.AddAsync(category);
        await SaveChanges();

        var result = await repository.Get(category.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        result.IsActive.Should().Be(category.IsActive);
        result.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        var exampleId = Guid.NewGuid();

        var task = async () => await repository
            .Get(exampleId, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NullReferenceException>()
            .WithMessage($"Category '{exampleId}' not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task Update()
    {
        var category = CategoryGenerator.GetCategory();
        await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var newName = Faker.Commerce.ProductName();
        category.Update(newName);
        category.Deactivate();
        repository = new CategoryRepository(dbContext);

        await repository.Update(category, CancellationToken.None);
        await SaveChanges();
        var dbCategory = await dbContext.Categories.FindAsync(category.Id);

        dbCategory!.IsActive.Should().BeFalse();
        dbCategory.Name.Should().Be(newName);
        dbCategory.CreatedAt.Should().Be(category.CreatedAt);
        dbCategory.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task Delete()
    {
        var category = CategoryGenerator.GetCategory();
        await dbContext.AddAsync(category);
        await SaveChanges();

        await repository.Delete(category, CancellationToken.None);
        await SaveChanges();
        var result = await CreateDbContext().Categories.FindAsync(category.Id);

        result.Should().BeNull();
    }

    #region Search
    [Theory(DisplayName = nameof(SearchWithOrder))]
    [Trait("Integration/Data", "Category - Repositories")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    [InlineData("id", SearchOrder.Asc)]
    [InlineData("id", SearchOrder.Desc)]
    [InlineData("createdat", SearchOrder.Asc)]
    [InlineData("createdat", SearchOrder.Desc)]
    [InlineData("", SearchOrder.Desc)]
    public async Task SearchWithOrder(string order, SearchOrder searchOrder)
    {
        var categories = CategoryGenerator.GetCategories(15);
        await dbContext.AddRangeAsync(categories);
        await SaveChanges();

        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, "", order, searchOrder);

        var result = await repository.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Items.Count.Should().Be(perPage);
    }

    [Fact(DisplayName = nameof(SearchWithTerm))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task SearchWithTerm()
    {
        var totalItens = 15;
        var categories = CategoryGenerator.GetCategories(totalItens);
        await dbContext.AddRangeAsync(categories);
        await SaveChanges();

        var searchTerm = categories.FirstOrDefault()!.Name;

        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, searchTerm, "name", SearchOrder.Desc);

        var result = await repository.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Filtred.Should().NotBe(0);
    }

    [Fact(DisplayName = nameof(SearcReturnsEmpty))]
    [Trait("Integration/Data", "Category - Repositories")]
    public async Task SearcReturnsEmpty()
    {
        var page = 1;
        var perPage = 10;
        var input = new SearchInput(page, perPage, "12312312", "", SearchOrder.Desc);

        var result = await repository.Search(input, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(page);
        result.Filtred.Should().Be(0);
        result.Items.Count.Should().Be(0);
    }
    #endregion
}
