using Application.Dtos.Video;
using Application.Interfaces.UseCases;
using Domain.Repository;

namespace Application.UseCases.Video;
public class GetVideo : IGetVideo
{
    private readonly IVideoRepository _repository;

    public GetVideo(IVideoRepository repository)
        => _repository = repository;

    public async Task<VideoOutput> Handle(
        GetVideoInput input,
        CancellationToken cancellationToken)
    {
        var video = await _repository.Get(input.id, cancellationToken);
        return VideoOutput.FromVideo(video);
    }
}
