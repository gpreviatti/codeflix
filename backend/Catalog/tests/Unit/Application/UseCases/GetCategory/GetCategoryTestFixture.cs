using Application.Interfaces.UseCases;
using Domain.Entity;
using Domain.Repository;
using Moq;
using Unit.Common;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryTestFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _respoitoryMock = new();

    protected IGetCategory _getCategory;

    public GetCategoryTestFixture()
    {
        _getCategory = new CategoryUseCase.GetCategory(_respoitoryMock.Object);
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

    public Category GetValidCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
