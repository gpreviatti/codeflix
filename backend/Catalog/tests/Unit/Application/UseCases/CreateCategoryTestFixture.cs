using Application.Dtos.Category;
using Application.Interfaces;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Domain.Repository;
using Moq;
using Unit.Common;

namespace Unit.Application.UseCases;

public class CreateCategoryTestFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _respoitoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    protected ICreateCategory _createCategory;

    public CreateCategoryTestFixture()
    {
        _createCategory = new CreateCategory(_respoitoryMock.Object, _unitOfWorkMock.Object);
    }

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public CreateCategoryInput GetValidCategoryInput() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
