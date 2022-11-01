using Domain.Repository;
using Infra.Data;
using Infra.Data.Repositories;

namespace Tests.Integration.Application.UseCases.Category;
public abstract class CategoryTestFixture : BaseFixture
{
    protected ICategoryRepository categoryRepository;
    protected IUnitOfWork unitOfWork;

    public CategoryTestFixture()
	{
		categoryRepository = new CategoryRepository(dbContext);
        unitOfWork = new UnitOfWork(dbContext);
    }
}
