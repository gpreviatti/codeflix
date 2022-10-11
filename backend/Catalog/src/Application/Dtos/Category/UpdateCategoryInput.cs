using Application.Messages;
using MediatR;

namespace Application.Dtos.Category;

public class UpdateCategoryInput : IRequest<BaseResponse<CategoryOutput>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? Is_Active { get; set; }

    public UpdateCategoryInput(
        Guid id,
        string name,
        string? description = null,
        bool? is_Active = null)
    {
        Id = id;
        Name = name;
        Description = description;
        Is_Active = is_Active;
    }
}
