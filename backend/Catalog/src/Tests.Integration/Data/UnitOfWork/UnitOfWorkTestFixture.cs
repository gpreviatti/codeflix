using Domain.Repository;

namespace Tests.Integration.Data.UnitOfWork;
public abstract class UnitOfWorkTestFixture : BaseFixture
{
    protected IUnitOfWork unitOfWork;

    public UnitOfWorkTestFixture()
    {
        unitOfWork = new Infra.Data.UnitOfWork(dbContext);
    }
}
