using Application.Messages;
using MediatR;

namespace Application.Dtos.Genre;
public class UpdateGenreInput : IRequest<BaseResponse<GenreOutput>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? Is_Active { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public UpdateGenreInput(
        Guid id,
        string name,
        bool? is_Active = null,
        List<Guid>? categoriesIds = null
    )
    {
        Id = id;
        Name = name;
        Is_Active = is_Active;
        CategoriesIds = categoriesIds;
    }
}
