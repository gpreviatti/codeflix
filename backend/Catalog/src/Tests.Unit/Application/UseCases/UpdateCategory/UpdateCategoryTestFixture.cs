using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Bogus;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.UpdateCategory;

public class UpdateCategoryTestFixture : CategoryBaseFixture
{
    protected readonly IUpdateCategory _updateCategory;

    public UpdateCategoryTestFixture()
    {
        _updateCategory = new CategoryUseCase.UpdateCategory(
            _repositoryMock.Object, _unitOfWorkMock.Object
        );
    }

    public UpdateCategoryInput GetValidInput(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        
        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();
        var tooLongName = Faker.Commerce.ProductName();

        while (tooLongName.Length <= 255)
            tooLongName = $"{tooLongName} {Faker.Commerce.ProductName()}";

        invalidInputTooLongName.Name = tooLongName;

        return invalidInputTooLongName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetValidInput();
        var tooLongDescription = Faker.Commerce.ProductDescription();

        while (tooLongDescription.Length <= 10000)
            tooLongDescription = $"{tooLongDescription} {Faker.Commerce.ProductDescription()}";

        invalidInputTooLongDescription.Description = tooLongDescription;
        
        return invalidInputTooLongDescription;
    }
}
