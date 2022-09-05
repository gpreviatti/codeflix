using Domain.Repository;
using FluentAssertions;
using Infra.Data.Repositories;

namespace Integration.Data.Repositories;
public class CategoryRepositoryTest : CategoryRepositoryTestFixture
{
    private readonly ICategoryRepository _repoistory;

    public CategoryRepositoryTest()
    {
        _repoistory = new CategoryRepository(dbContext);
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Data", "Category - Repositories")]
    public async Task Insert()
    {
        var category = GetCategory();

        await _repoistory.Insert(category, CancellationToken.None);
        await SaveChanges();
        
        var dbCategory = await dbContext.Categories.FindAsync(category.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(category.Name);
        dbCategory!.Description.Should().Be(category.Description);
        dbCategory!.IsActive.Should().Be(category.IsActive);
        dbCategory!.CreatedAt.Should().Be(category.CreatedAt);
    }
}
