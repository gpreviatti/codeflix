using Application.Dtos.Video;
using System.Text;
using Tests.Common.Generators.Entities;

namespace Tests.Common.Generators.Dtos;
public class UpdateVideoInputGenerator : VideoGenerator
{
    public static UpdateVideoInput CreateValidInput(
        List<Guid>? categoriesIds = null,
        List<Guid>? genresIds = null,
        List<Guid>? castMembersIds = null,
        FileInput? thumb = null,
        FileInput? banner = null,
        FileInput? thumbHalf = null
    ) => new(
        Guid.NewGuid(),
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating(),
        genresIds,
        categoriesIds,
        castMembersIds,
        banner,
        thumb,
        thumbHalf
    );

    public static UpdateVideoInput CreateValidInputWithAllImages(Guid id) => new(
        id,
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating(),
        null,
        null,
        null,
        GetValidImageFileInput(),
        GetValidImageFileInput(),
        GetValidImageFileInput()
    );

    public static UpdateVideoInput CreateValidInputWithAllMedias(Guid id) => new(
        id,
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating(),
        null,
        null,
        null,
        null,
        null,
        null
    );

    public static FileInput GetValidImageFileInput()
    {
        var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
        var fileInput = new FileInput("jpg", exampleStream);
        return fileInput;
    }

    public static FileInput GetValidMediaFileInput()
    {
        var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
        var fileInput = new FileInput("mp4", exampleStream);
        return fileInput;
    }
}
