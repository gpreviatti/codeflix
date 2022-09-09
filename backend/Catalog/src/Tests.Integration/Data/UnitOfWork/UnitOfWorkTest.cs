using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Data.UnitOfWork;

public class UnitOfWorkTest : UnitOfWorkTestFixture
{
    [Fact(DisplayName = nameof(Commit))]
    [Trait("Data", "UnitOfWork")]
    public async Task Commit()
    {
        unitOfWork = new Infra.Data.UnitOfWork(dbContext);
        var categories = CategoryGenerator.GetCategories(2);
        await dbContext.Categories.AddRangeAsync(categories);

        await unitOfWork.Commit(CancellationToken.None);
        var dbCategories = await dbContext.Categories
            .AsNoTracking().ToListAsync();

        dbCategories
            .Should().NotBeNull()
            .And.HaveCount(categories.Count);
    }

    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Data", "UnitOfWork")]
    public async Task Rollback()
    {
        var task = async () => await unitOfWork
            .Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}
