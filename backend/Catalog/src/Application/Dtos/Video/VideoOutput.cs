using Domain.Extensions;
using DomainEntity = Domain.Entity;

namespace Application.Dtos.Video;
public record VideoOutput(
    Guid Id,
    DateTime CreatedAt,
    string Title,
    bool Published,
    string Description,
    string Rating,
    int YearLaunched,
    bool Opened,
    int Duration,
    IReadOnlyCollection<VideoOutputRelatedAggregate> Categories,
    IReadOnlyCollection<VideoOutputRelatedAggregate> Genres,
    IReadOnlyCollection<VideoOutputRelatedAggregate> CastMembers,

    string? ThumbFileUrl,
    string? BannerFileUrl,
    string? ThumbHalfFileUrl,
    string? VideoFileUrl,
    string? TrailerFileUrl)
{
    public static VideoOutput FromVideo(DomainEntity.Video video) => new(
        video.Id,
        video.CreatedAt,
        video.Title,
        video.Published,
        video.Description,
        video.Rating.ToStringSignal(),
        video.YearLaunched,
        video.Opened,
        video.Duration,
        video.Categories.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
        video.Genres.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
        video.CastMembers.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
        video.Thumb?.Path,
        video.Banner?.Path,
        video.ThumbHalf?.Path,
        video.Media?.FilePath,
        video.Trailer?.FilePath);

    public static VideoOutput FromVideo(
        DomainEntity.Video video,
        IReadOnlyList<DomainEntity.Category>? categories = null,
        IReadOnlyCollection<DomainEntity.Genre>? genres = null
    ) => new(
        video.Id,
        video.CreatedAt,
        video.Title,
        video.Published,
        video.Description,
        video.Rating.ToStringSignal(),
        video.YearLaunched,
        video.Opened,
        video.Duration,
        video.Categories.Select(id =>
            new VideoOutputRelatedAggregate(
                id,
                categories?.FirstOrDefault(category => category.Id == id)?.Name
            )).ToList(),
        video.Genres.Select(id =>
            new VideoOutputRelatedAggregate(
                id,
                genres?.FirstOrDefault(genre => genre.Id == id)?.Name
            )).ToList(),
        video.CastMembers.Select(id => new VideoOutputRelatedAggregate(id)).ToList(),
        video.Thumb?.Path,
        video.Banner?.Path,
        video.ThumbHalf?.Path,
        video.Media?.FilePath,
        video.Trailer?.FilePath
    );
}

public record VideoOutputRelatedAggregate(Guid Id, string? Name = null);