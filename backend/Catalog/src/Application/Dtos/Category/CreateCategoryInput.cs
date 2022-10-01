using MediatR;

namespace Application.Dtos.Category;

public class CreateCategoryInput : IRequest<CategoryOutput>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Is_Active { get; set; }

    public CreateCategoryInput(string name, string description = null!, bool isActive = true)
    {
        Name = name;
        Description = description ?? "";
        Is_Active = isActive;
    }
}
