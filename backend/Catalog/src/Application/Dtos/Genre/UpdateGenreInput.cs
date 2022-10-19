using Application.Messages;
using MediatR;

namespace Application.Dtos.Genre;
public class UpdateGenreInput : IRequest<BaseResponse<GenreOutput>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public UpdateGenreInput(
        Guid id,
        string name,
        bool? isActive = null,
        List<Guid>? categoriesIds = null
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}
