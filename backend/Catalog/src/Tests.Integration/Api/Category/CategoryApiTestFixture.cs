using Microsoft.EntityFrameworkCore;
using Tests.Integration.Api.Common;
using DomainEntity = Domain.Entity;

namespace Tests.Integration.Api.Category;
public class CategoryApiTestFixture : BaseApiTestFixture
{
    protected readonly string RESOURCE_URL = "/categories";

    public async Task<DomainEntity.Category?> GetById(Guid id) => await dbContext
        .Categories.FirstOrDefaultAsync(c => c.Id.Equals(id));
}
