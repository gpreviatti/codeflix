using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Category;

public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse<CategoryOutput>> Handle(
        GetCategoryInput input,
        CancellationToken cancellationToken
    )
    {
        var entity = await _categoryRepository.Get(input.Id, cancellationToken);

        return new(CategoryOutput.FromCategory(entity));
    }
}
