using MediatR;

namespace Application.Dtos.Video;
public record GetVideoInput(Guid id) : IRequest<VideoOutput>;