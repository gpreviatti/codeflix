using MediatR;

namespace Application.Dtos.Genre;
public class DeleteGenreInput : IRequest
{
    public DeleteGenreInput(Guid id) => Id = id;

    public Guid Id { get; set; }
}
