using Domain.Repository;

namespace Infra.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly CatalogDbContext _context;

    public UnitOfWork(CatalogDbContext context) => _context = context;

    public async Task<bool> Commit(CancellationToken cancellationToken) =>
        await _context.SaveChangesAsync(cancellationToken) > 0;

    public Task Rollback(CancellationToken cancellationToken) => Task.CompletedTask;
}
