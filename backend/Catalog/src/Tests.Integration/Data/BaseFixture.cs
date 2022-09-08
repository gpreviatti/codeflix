using Bogus;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Integration.Data;
public class BaseFixture : IDisposable
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
}
