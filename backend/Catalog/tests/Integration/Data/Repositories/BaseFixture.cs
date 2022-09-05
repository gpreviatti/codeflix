using Bogus;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Integration.Data.Repositories;
public class BaseFixture
{
    protected Faker Faker { get; set; }
	protected readonly CatalogDbContext dbContext;

    public BaseFixture()
	{
		Faker = new Faker("pt_BR");
		dbContext = CreateDbContext();
	}

	public CatalogDbContext CreateDbContext() => new(
		new DbContextOptionsBuilder<CatalogDbContext>()
		.UseInMemoryDatabase("fc-db-integration-tests")
		.Options
	);

	public async Task<int> SaveChanges() => await dbContext.SaveChangesAsync();
}
