using Application.Dtos.Category;
using Application.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryInputValidatorTest
{
    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void ValidationOk()
    {
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidation();

        var validationResult = validator.TestValidate(validInput);

        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidation();

        var validationResult = validator.TestValidate(invalidInput);

        validationResult
        .ShouldHaveAnyValidationError()
        .WithErrorMessage("'Id' must not be empty.");
    }
}
