using Application.Messages;
using MediatR;

namespace Application.Dtos.Genre;

public class CreateGenreInput : IRequest<BaseResponse<GenreOutput>>
{
    public CreateGenreInput(
        string name,
        bool is_Active,
        List<Guid>? categories_Ids = null
    )
    {
        Name = name;
        Is_Active = is_Active;
        Categories_Ids = categories_Ids;
    }

    public string Name { get; set; }
    public bool Is_Active { get; set; }
    public List<Guid>? Categories_Ids { get; set; }
}
