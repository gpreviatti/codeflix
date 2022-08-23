namespace Application.Interfaces;

public interface IUnitOfWork
{
    public Task<bool> Commit(CancellationToken cancellationToken);
}
