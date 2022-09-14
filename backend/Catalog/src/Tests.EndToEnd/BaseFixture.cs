using Bogus;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators;

namespace Tests.EndToEnd;
public abstract class BaseFixture : IDisposable
{
    protected Faker Faker { get; set; } = CommonGenerator.GetFaker();
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
}
