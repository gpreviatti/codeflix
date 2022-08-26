using Application.Dtos.Category;
using Application.Interfaces;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Domain.Repository;
using Moq;
using Unit.Common;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.CreateCategory;

public class CreateCategoryTestFixture : BaseFixture
{
    protected readonly Mock<ICategoryRepository> _respoitoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    protected ICreateCategory _createCategory;

    public CreateCategoryTestFixture()
    {
        _createCategory = new CategoryUseCase.CreateCategory(_respoitoryMock.Object, _unitOfWorkMock.Object);
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

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var inputShortName = GetValidCategoryInput();
        inputShortName.Name = inputShortName.Name[..2];
        return inputShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var inputTooLongName = GetValidCategoryInput();
        inputTooLongName.Name = Faker.Lorem.Letter(256);
        return inputTooLongName;
    }

    public CreateCategoryInput GetInvalidInputNameNull()
    {
        var inputNullName = GetValidCategoryInput();
        inputNullName.Name = null!;
        return inputNullName;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var inputNullDescription = GetValidCategoryInput();
        inputNullDescription.Description = null!;
        return inputNullDescription;
    }

    public CreateCategoryInput GetInvalidInputDescriptionTooLongDescription()
    {
        var inputTooLongDescription = GetValidCategoryInput();
        inputTooLongDescription.Description = Faker.Lorem.Letter(10001);
        return inputTooLongDescription;
    }
}
