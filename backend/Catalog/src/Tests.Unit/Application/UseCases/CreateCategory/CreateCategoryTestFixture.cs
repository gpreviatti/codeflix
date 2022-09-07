using Application.Dtos.Category;
using Application.Interfaces;
using Application.Interfaces.UseCases;
using Domain.Repository;
using Moq;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.CreateCategory;

public class CreateCategoryTestFixture : CategoryBaseFixture
{
    protected ICreateCategory _createCategory;

    public CreateCategoryTestFixture()
    {
        _createCategory = new CategoryUseCase.CreateCategory(_repositoryMock.Object, _unitOfWorkMock.Object);
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
