using Application.Dtos.Video;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IListVideos : IRequestHandler<ListVideosInput, BasePaginatedResponse<List<VideoOutput>>> { }
