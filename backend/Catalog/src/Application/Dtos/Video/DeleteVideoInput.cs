using MediatR;

namespace Application.Dtos.Video;
public record DeleteVideoInput(Guid id) : IRequest;
