using Bogus;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators;

namespace Tests.Integration;
public abstract class BaseFixture
{
    protected Faker Faker { get; set; } = CommonGenerator.GetFaker();
    protected CatalogDbContext dbContext;

    public BaseFixture()
    {
        dbContext = CreateDbContext();
    }

    public static CatalogDbContext CreateDbContext()
    {
        var connectionString = $"Server=localhost;Port=3306;Uid=root;Pwd=codeflix;Database=catalog_test";
        return new(
            new DbContextOptionsBuilder<CatalogDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options
        );
    }

    public async Task<int> SaveChanges() => await dbContext.SaveChangesAsync();
}
