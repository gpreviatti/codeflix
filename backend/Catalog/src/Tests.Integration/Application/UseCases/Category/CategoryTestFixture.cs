using Domain.Repository;
using Infra.Data.Repositories;
using Tests.Integration.Data;

namespace Tests.Integration.Application.UseCases.Category;
public abstract class CategoryTestFixture : BaseFixture
{
    protected readonly ICategoryRepository _categoryRepository;

	public CategoryTestFixture()
	{
		_categoryRepository = new CategoryRepository(dbContext);
	}
}
