using Application.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Tests.Common.Generators.Dtos;

namespace Unit.Application.UseCases.UpdateCategory;

public class UpdateCategoryInputValidationTest
{
    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = UpdateCategoryInputGenerator.GetCategory();
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.TestValidate(input);

        validateResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var input = UpdateCategoryInputGenerator.GetCategory(Guid.Empty);
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.TestValidate(input);

        validateResult
            .ShouldHaveAnyValidationError()
            .WithErrorMessage("'Id' must not be empty.");
    }
}
