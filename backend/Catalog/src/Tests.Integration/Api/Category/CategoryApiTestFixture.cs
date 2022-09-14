using Microsoft.EntityFrameworkCore;
using Tests.Integration.Api.Common;
using DomainEntity = Domain.Entity;

namespace Tests.Integration.Api.Category;
public class CategoryApiTestFixture : BaseApiTestFixture
{
    protected readonly string RESOURCE_URL = "/categories";

    protected readonly DbSet<DomainEntity.Category> dbContextCategory;

    public async Task<DomainEntity.Category?> GetById(Guid id) => await dbContextCategory
        .AsNoTracking().FirstOrDefaultAsync(c => c.Id.Equals(id));
}
