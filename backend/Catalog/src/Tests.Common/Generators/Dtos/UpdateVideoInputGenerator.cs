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
        FileInput? thumbHalf = null,
        FileInput? media = null,
        FileInput? trailer = null
    ) => new(
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating(),
        categoriesIds,
        genresIds,
        castMembersIds,
        thumb,
        banner,
        thumbHalf,
        media,
        trailer
    );

    public static UpdateVideoInput CreateValidInputWithAllImages() => new(
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

    public static UpdateVideoInput CreateValidInputWithAllMedias() => new(
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
        null,
        GetValidMediaFileInput(),
        GetValidMediaFileInput()
    );

    public static IEnumerator<object[]> GetEnumerator()
    {
        var invalidInputsList = new List<object[]>();
        const int totalInvalidCases = 4;

        for (int index = 0; index < totalInvalidCases * 2; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[] {
                        new CreateVideoInput(
                            "",
                            GetValidDescription(),
                            GetValidYearLaunched(),
                            GetRandomBoolean(),
                            GetRandomBoolean(),
                            GetValidDuration(),
                            GetRandomRating()
                        ),
                        "'Title' is required"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[] {
                        new CreateVideoInput(
                            GetValidTitle(),
                            "",
                            GetValidYearLaunched(),
                            GetRandomBoolean(),
                            GetRandomBoolean(),
                            GetValidDuration(),
                            GetRandomRating()
                        ),
                        "'Description' is required"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[] {
                        new CreateVideoInput(
                            GetTooLongTitle(),
                            GetValidDescription(),
                            GetValidYearLaunched(),
                            GetRandomBoolean(),
                            GetRandomBoolean(),
                            GetValidDuration(),
                            GetRandomRating()
                        ),
                        "'Title' should be less or equal 255 characters long"
                    });
                    break;
                case 3:
                    invalidInputsList.Add(new object[] {
                        new CreateVideoInput(
                            GetValidTitle(),
                            GetTooLongDescription(),
                            GetValidYearLaunched(),
                            GetRandomBoolean(),
                            GetRandomBoolean(),
                            GetValidDuration(),
                            GetRandomRating()
                        ),
                        "'Description' should be less or equal 4000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList.GetEnumerator();
    }

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
