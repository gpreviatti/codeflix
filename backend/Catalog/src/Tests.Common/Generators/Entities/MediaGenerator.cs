using Domain.Enum;
using System.Text;
using DomainEntity = Domain.Entity;

namespace Tests.Common.Generators.Entities;
public class MediaGenerator : CommonGenerator
{
    public static DomainEntity.Video GetValidVideo() => new DomainEntity.Video(
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating()
    );

    public static DomainEntity.Video GetValidVideoWithAllProperties()
    {
        var video = new DomainEntity.Video(
            GetValidTitle(),
            GetValidDescription(),
            GetValidYearLaunched(),
            GetRandomBoolean(),
            GetRandomBoolean(),
            GetValidDuration(),
            GetRandomRating()
        );

        video.UpdateBanner(GetValidImagePath());
        video.UpdateThumb(GetValidImagePath());
        video.UpdateThumbHalf(GetValidImagePath());

        video.UpdateMedia(GetValidMediaPath());
        video.UpdateTrailer(GetValidMediaPath());

        var random = new Random();
        Enumerable.Range(1, random.Next(2, 5)).ToList()
            .ForEach(_ => video.AddCastMember(Guid.NewGuid()));
        Enumerable.Range(1, random.Next(2, 5)).ToList()
            .ForEach(_ => video.AddCategory(Guid.NewGuid()));
        Enumerable.Range(1, random.Next(2, 5)).ToList()
            .ForEach(_ => video.AddGenre(Guid.NewGuid()));

        return video;
    }

    public static Rating GetRandomRating()
    {
        var enumValue = Enum.GetValues<Rating>();
        var random = new Random();
        return enumValue[random.Next(enumValue.Length)];
    }

    public static string GetValidTitle() => GetFaker().Lorem.Letter(100);
    public static string GetValidDescription() => GetFaker().Commerce.ProductDescription();
    public static string GetTooLongDescription() => GetFaker().Lorem.Letter(4001);

    public static int GetValidYearLaunched() => GetFaker().Date.BetweenDateOnly(
        new DateOnly(1960, 1, 1),
        new DateOnly(2022, 1, 1)
    ).Year;

    public static int GetValidDuration() => (new Random()).Next(100, 300);
    public static string GetTooLongTitle() => GetFaker().Lorem.Letter(400);
    public static string GetValidImagePath() => GetFaker().Image.PlaceImgUrl();

    public static string GetValidMediaPath()
    {
        var exampleMedias = new string[]
        {
            "https://www.googlestorage.com/file-example.mp4",
            "https://www.storage.com/another-example-of-video.mp4",
            "https://www.S3.com.br/example.mp4",
            "https://www.glg.io/file.mp4"
        };
        var random = new Random();
        return exampleMedias[random.Next(exampleMedias.Length)];
    }

    public static DomainEntity.Media GetValidMedia() => new(GetValidMediaPath());
}
