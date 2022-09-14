using Domain.Repository;
using Infra.Data;
using Infra.Data.Repositories;

namespace Tests.Integration.Application.UseCases.Category;
public abstract class CategoryTestFixture : BaseFixture
{
    protected ICategoryRepository _categoryRepository;
    protected IUnitOfWork _unitOfWork;

    public CategoryTestFixture()
	{
		_categoryRepository = new CategoryRepository(dbContext);
        _unitOfWork = new UnitOfWork(dbContext);
    }
}
