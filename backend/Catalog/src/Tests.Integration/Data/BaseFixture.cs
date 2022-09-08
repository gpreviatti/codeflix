using Bogus;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Entity = Domain.Entity;

namespace Tests.Integration.Data;
public abstract class BaseFixture : IDisposable
{
    protected Faker Faker { get; set; } = new Faker("pt_BR");
    protected CatalogDbContext dbContext;

    public BaseFixture()
    {
        dbContext = CreateDbContext(Guid.NewGuid());
    }

    public static CatalogDbContext CreateDbContext(Guid guid) => new(
        new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase($"fc-db-integration-tests-{guid}")
            .Options
    );

    public async Task<int> SaveChanges() => await dbContext.SaveChangesAsync();

    public void Dispose() => dbContext.Database.EnsureDeleted();

    #region Category data generators
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public Entity.Category GetValidCategory() =>
        new(GetValidCategoryName(), GetValidCategoryDescription());

    public List<Entity.Category> GetCategories(int length = 10) =>
        Enumerable.Range(1, length).Select(_ => GetValidCategory()).ToList();
    #endregion
}
