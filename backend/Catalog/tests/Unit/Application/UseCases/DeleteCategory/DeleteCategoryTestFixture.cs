using Application.Interfaces.UseCases;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.DeleteCategory;

public class DeleteCategoryTestFixture : CategoryBaseFixture
{
    protected readonly IDeleteCategory _deleteCategory;

    public DeleteCategoryTestFixture()
    {
        _deleteCategory = new CategoryUseCase.DeleteCategory(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }
}
