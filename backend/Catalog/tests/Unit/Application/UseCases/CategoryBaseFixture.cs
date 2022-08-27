using Application.Interfaces;
using Domain.Entity;
using Domain.Repository;
using Moq;
using Unit.Common;

namespace Unit.Application.UseCases;

public class CategoryBaseFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _repositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

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
        var categoryDescription =
            Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription =
                categoryDescription[..10_000];
        return categoryDescription;
    }

    public Category GetExampleCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
