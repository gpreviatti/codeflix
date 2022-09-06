using Bogus;
using Domain.SeedWork;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Integration.Data.Repositories;
public class BaseFixture
{
	protected Faker Faker { get; set; } = new Faker("pt_BR");
    protected CatalogDbContext dbContext;

    public BaseFixture()
	{
		dbContext = CreateDbContext();
	}

	public CatalogDbContext CreateDbContext() => new(
		new DbContextOptionsBuilder<CatalogDbContext>()
		.UseInMemoryDatabase("fc-db-integration-tests")
		.Options
	);

    public async Task<int> SaveChanges() => await dbContext.SaveChangesAsync();
}
