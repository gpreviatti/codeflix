using Application.Messages;
using MediatR;

namespace Application.Dtos.Genre;

public class CreateGenreInput : IRequest<BaseResponse<GenreOutput>>
{
    public CreateGenreInput(
        string name,
        bool isActive,
        List<Guid>? categoriesIds = null
    )
    {
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }

    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }
}
