using Application.Interfaces.UseCases;
using Domain.Entity;
using Domain.Repository;
using Moq;
using Unit.Common;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryTestFixture : CategoryBaseFixture
{
    protected readonly Mock<ICategoryRepository> _respoitoryMock = new();

    protected IGetCategory _getCategory;

    public GetCategoryTestFixture()
    {
        _getCategory = new CategoryUseCase.GetCategory(_respoitoryMock.Object);
    }
}
