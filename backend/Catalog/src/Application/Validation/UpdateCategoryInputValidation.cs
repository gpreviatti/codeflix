using Application.Dtos.Category;
using FluentValidation;

namespace Application.Validation;

public class UpdateCategoryInputValidation : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidation() => RuleFor(x => x.Id).NotEmpty();
}
