using Application.Validation;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Unit.Application.UseCases.UpdateCategory;

public class UpdateCategoryInputValidationTest : UpdateCategoryTestFixture
{
    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var input = GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }


    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = GetValidInput();
        var validator = new UpdateCategoryInputValidation();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}
