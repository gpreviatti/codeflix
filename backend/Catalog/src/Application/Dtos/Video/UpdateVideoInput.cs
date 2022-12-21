using Domain.Enum;
using MediatR;

namespace Application.Dtos.Video;
public record UpdateVideoInput(
    Guid Id,
    string Title,
    string Description,
    int YearLaunched,
    bool Opened,
    bool Published,
    int Duration,
    Rating Rating,
    List<Guid>? GenresIds = null,
    List<Guid>? CategoriesIds = null,
    List<Guid>? CastMembersIds = null,
    FileInput? Banner = null,
    FileInput? Thumb = null,
    FileInput? ThumbHalf = null
) : IRequest<VideoOutput>;