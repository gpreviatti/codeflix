using Application.Dtos.Category;
using FluentValidation;

namespace Application.Validation;

public class GetCategoryInputValidation : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidation() => RuleFor(x => x.Id).NotEmpty();
}
