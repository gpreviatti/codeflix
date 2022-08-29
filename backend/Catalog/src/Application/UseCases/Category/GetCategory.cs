using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Domain.Repository;

namespace Application.UseCases.Category;

public class GetCategory : IGetCategory
{
	private readonly ICategoryRepository _categoryRepository;

	public GetCategory(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<CategoryOutput> Handle(
		GetCategoryInput input,
		CancellationToken cancellationToken
	)
	{
		var category = await _categoryRepository.Get(input.Id, cancellationToken);

		return CategoryOutput.FromCategory(category);
	}
}
