using Application.Validation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Unit.Application.UseCases.UpdateCategory;

public class UpdateCategoryInputValidationTest : UpdateCategoryTestFixture
{
    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = GetValidInput();
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.TestValidate(input);

        validateResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var input = GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.TestValidate(input);

        validateResult
            .ShouldHaveAnyValidationError()
            .WithErrorMessage("'Id' must not be empty.");
    }
}
