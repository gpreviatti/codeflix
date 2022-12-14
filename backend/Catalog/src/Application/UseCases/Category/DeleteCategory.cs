using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.Category;

public class DeleteCategory : IDeleteCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategory(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    )
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _categoryRepository.Get(request.Id, cancellationToken);

        await _categoryRepository.Delete(entity, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);

        return Unit.Value;
    }
}
