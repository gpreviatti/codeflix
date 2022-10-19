using Application.Messages;
using MediatR;

namespace Application.Dtos.Genre;
public class GetGenreInput : IRequest<BaseResponse<GenreOutput>>
{
    public GetGenreInput(Guid id) => Id = id;

    public Guid Id { get; set; }
}
