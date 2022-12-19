using Domain.Entity;
using Domain.Enum;
using Tests.Common.Generators.Entities;

namespace Tests.Unit.Domain.Entity;
public class MediaTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Media - Entities")]
    public void Instantiate()
    {
        var expectedFilePath = MediaGenerator.GetValidMediaPath();

        var media = new Media(expectedFilePath);

        media.FilePath.Should().Be(expectedFilePath);
        media.Status.Should().Be(MediaStatus.Pending);
    }

    [Fact(DisplayName = nameof(UpdateAsSentToEncode))]
    [Trait("Domain", "Media - Entities")]
    public void UpdateAsSentToEncode()
    {
        var media = MediaGenerator.GetValidMedia();

        media.UpdateAsSentToEncode();

        media.Status.Should().Be(MediaStatus.Processing);
    }

    [Fact(DisplayName = nameof(UpdateAsEncoded))]
    [Trait("Domain", "Media - Entities")]
    public void UpdateAsEncoded()
    {
        var media = MediaGenerator.GetValidMedia();
        var encodedExamplePath = MediaGenerator.GetValidMediaPath();
        media.UpdateAsSentToEncode();

        media.UpdateAsEncoded(encodedExamplePath);

        media.Status.Should().Be(MediaStatus.Completed);
        media.EncodedPath.Should().Be(encodedExamplePath);
    }
}
