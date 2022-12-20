using MediatR;

namespace Application.Dtos.Video;
public record UploadMediasInput(Guid id, FileInput? VideoFile, FileInput? TrailerFile) : IRequest;
