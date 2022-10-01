using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Domain.Repository;

namespace Application.UseCases.Category;

public class UpdateCategory : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategory(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    )
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutput> Handle(
        UpdateCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository
                .Get(request.Id, cancellationToken);

        category.Update(request.Name, request.Description);

        if (request.Is_Active != null && request.Is_Active != category.IsActive)
        {
            if ((bool)request.Is_Active!) category.Activate();
            else category.Deactivate();
        }

        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryOutput.FromCategory(category);
    }
}
