using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Domain.Repository;

namespace Application.UseCases.Category;

public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<CategoryOutput>> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken
    )
    {
        Domain.Entity.Category category = new(
            input.Name,
            input.Description,
            input.Is_Active
        );

        await _categoryRepository.Insert(category, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return new(CategoryOutput.FromCategory(category));
    }
}
