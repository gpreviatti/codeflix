using Domain.Enum;
using Domain.Excpetions;
using Domain.Validation;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;

namespace Tests.Unit.Domain.Entity;
public class VideoTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Video - Aggregate")]
    public void Instantiate()
    {
        var expectedTitle = VideoGenerator.GetValidTitle();
        var expectedDescription = VideoGenerator.GetValidDescription();
        var expectedYearLaunched = VideoGenerator.GetValidYearLaunched();
        var expectedOpened = VideoGenerator.GetRandomBoolean();
        var expectedPublished = VideoGenerator.GetRandomBoolean();
        var expectedDuration = VideoGenerator.GetValidDuration();
        var expectedRating = VideoGenerator.GetRandomRating();

        var expectedCreatedDate = DateTime.Now;
        var video = new DomainEntity.Video(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.Duration.Should().Be(expectedDuration);
        video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10));
        video.Thumb.Should().BeNull();
        video.ThumbHalf.Should().BeNull();
        video.Banner.Should().BeNull();
        video.Media.Should().BeNull();
        video.Trailer.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ValidateWhenValidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateWhenValidState()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var notificationHandler = new NotificationValidationHandler();

        validVideo.Validate(notificationHandler);

        notificationHandler.HasErrors().Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ValidateWithErrorWhenInvalidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateWithErrorWhenInvalidState()
    {
        var invalidVideo = new DomainEntity.Video(
            VideoGenerator.GetTooLongTitle(),
            VideoGenerator.GetTooLongDescription(),
            VideoGenerator.GetValidYearLaunched(),
            VideoGenerator.GetRandomBoolean(),
            VideoGenerator.GetRandomBoolean(),
            VideoGenerator.GetValidDuration(),
            VideoGenerator.GetRandomRating()
        );
        var notificationHandler = new NotificationValidationHandler();

        invalidVideo.Validate(notificationHandler);

        notificationHandler.HasErrors().Should().BeTrue();
        notificationHandler.Errors.Should()
            .BeEquivalentTo(new List<ValidationError>()
            {
                new ValidationError("'Title' should be less or equal 255 characters long"),
                new ValidationError("'Description' should be less or equal 4000 characters long")
            });
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Video - Aggregate")]
    public void Update()
    {
        var expectedTitle = VideoGenerator.GetValidTitle();
        var expectedDescription = VideoGenerator.GetValidDescription();
        var expectedYearLaunched = VideoGenerator.GetValidYearLaunched();
        var expectedOpened = VideoGenerator.GetRandomBoolean();
        var expectedPublished = VideoGenerator.GetRandomBoolean();
        var expectedDuration = VideoGenerator.GetValidDuration();
        var expectedRating = VideoGenerator.GetRandomRating();
        var video = VideoGenerator.GetValidVideo();

        video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.Duration.Should().Be(expectedDuration);
    }

    [Fact(DisplayName = nameof(UpdateWithRating))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateWithRating()
    {
        var expectedTitle = VideoGenerator.GetValidTitle();
        var expectedDescription = VideoGenerator.GetValidDescription();
        var expectedYearLaunched = VideoGenerator.GetValidYearLaunched();
        var expectedOpened = VideoGenerator.GetRandomBoolean();
        var expectedPublished = VideoGenerator.GetRandomBoolean();
        var expectedDuration = VideoGenerator.GetValidDuration();
        var expectedRating = VideoGenerator.GetRandomRating();
        var video = VideoGenerator.GetValidVideo();

        video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.Duration.Should().Be(expectedDuration);
        video.Rating.Should().Be(expectedRating);
    }

    [Fact(DisplayName = nameof(UpdateWithoutRatingDoesntChangeTheRating))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateWithoutRatingDoesntChangeTheRating()
    {
        var video = VideoGenerator.GetValidVideo();
        var expectedRating = video.Rating;

        video.Update(
            VideoGenerator.GetValidTitle(),
            VideoGenerator.GetValidDescription(),
            VideoGenerator.GetValidYearLaunched(),
            VideoGenerator.GetRandomBoolean(),
            VideoGenerator.GetRandomBoolean(),
            VideoGenerator.GetValidDuration()
        );

        video.Rating.Should().Be(expectedRating);
    }

    [Fact(DisplayName = nameof(ValidateStillValidatingAfterUpdateToValidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateStillValidatingAfterUpdateToValidState()
    {
        var expectedTitle = VideoGenerator.GetValidTitle();
        var expectedDescription = VideoGenerator.GetValidDescription();
        var expectedYearLaunched = VideoGenerator.GetValidYearLaunched();
        var expectedOpened = VideoGenerator.GetRandomBoolean();
        var expectedPublished = VideoGenerator.GetRandomBoolean();
        var expectedDuration = VideoGenerator.GetValidDuration();
        var expectedRating = VideoGenerator.GetRandomRating();
        var video = VideoGenerator.GetValidVideo();
        video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );
        var notificationHandler = new NotificationValidationHandler();

        video.Validate(notificationHandler);

        notificationHandler.HasErrors().Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ValidateGenerateErrorsAfterUpdateToInvalidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateGenerateErrorsAfterUpdateToInvalidState()
    {
        var expectedTitle = VideoGenerator.GetTooLongTitle();
        var expectedDescription = VideoGenerator.GetTooLongDescription();
        var expectedYearLaunched = VideoGenerator.GetValidYearLaunched();
        var expectedOpened = VideoGenerator.GetRandomBoolean();
        var expectedPublished = VideoGenerator.GetRandomBoolean();
        var expectedDuration = VideoGenerator.GetValidDuration();
        var expectedRating = VideoGenerator.GetRandomRating();
        var video = VideoGenerator.GetValidVideo();
        video.Update(
            expectedTitle,
            expectedDescription,
            expectedYearLaunched,
            expectedOpened,
            expectedPublished,
            expectedDuration,
            expectedRating
        );
        var notificationHandler = new NotificationValidationHandler();

        video.Validate(notificationHandler);

        notificationHandler.HasErrors().Should().BeTrue();
        notificationHandler.Errors.Should().HaveCount(2);
        notificationHandler.Errors.Should()
            .BeEquivalentTo(new List<ValidationError>()
        {
            new ValidationError("'Title' should be less or equal 255 characters long"),
            new ValidationError("'Description' should be less or equal 4000 characters long")
        });
    }

    [Fact(DisplayName = nameof(UpdateThumb))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateThumb()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validImagePath = VideoGenerator.GetValidImagePath();

        validVideo.UpdateThumb(validImagePath);

        validVideo.Thumb.Should().NotBeNull();
        validVideo.Thumb!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateThumbHalf))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateThumbHalf()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validImagePath = VideoGenerator.GetValidImagePath();

        validVideo.UpdateThumbHalf(validImagePath);

        validVideo.ThumbHalf.Should().NotBeNull();
        validVideo.ThumbHalf!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateBanner))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateBanner()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validImagePath = VideoGenerator.GetValidImagePath();

        validVideo.UpdateBanner(validImagePath);

        validVideo.Banner.Should().NotBeNull();
        validVideo.Banner!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateMedia()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validPath = VideoGenerator.GetValidMediaPath();

        validVideo.UpdateMedia(validPath);

        validVideo.Media.Should().NotBeNull();
        validVideo.Media!.FilePath.Should().Be(validPath);
    }

    [Fact(DisplayName = nameof(UpdateAsSentToEncode))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsSentToEncode()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validPath = VideoGenerator.GetValidMediaPath();
        validVideo.UpdateMedia(validPath);

        validVideo.UpdateAsSentToEncode();

        validVideo.Media!.Status.Should().Be(MediaStatus.Processing);
    }

    [Fact(DisplayName = nameof(UpdateAsSentToEncodeThrowsWhenThereIsNoMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsSentToEncodeThrowsWhenThereIsNoMedia()
    {
        var validVideo = VideoGenerator.GetValidVideo();

        var action = () => validVideo.UpdateAsSentToEncode();

        action.Should().Throw<EntityValidationException>()
            .WithMessage("There is no Media");
    }

    [Fact(DisplayName = nameof(UpdateAsEncoded))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsEncoded()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validPath = VideoGenerator.GetValidMediaPath();
        var validEncodedPath = VideoGenerator.GetValidMediaPath();
        validVideo.UpdateMedia(validPath);

        validVideo.UpdateAsEncoded(validEncodedPath);

        validVideo.Media!.Status.Should().Be(MediaStatus.Completed);
        validVideo.Media!.EncodedPath.Should().Be(validEncodedPath);
    }

    [Fact(DisplayName = nameof(UpdateAsEncodedThrowsWhenThereIsNoMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsEncodedThrowsWhenThereIsNoMedia()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validPath = VideoGenerator.GetValidMediaPath();

        var action = () => validVideo.UpdateAsEncoded(validPath);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("There is no Media");
    }

    [Fact(DisplayName = nameof(UpdateTrailer))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateTrailer()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var validPath = VideoGenerator.GetValidMediaPath();

        validVideo.UpdateTrailer(validPath);

        validVideo.Trailer.Should().NotBeNull();
        validVideo.Trailer!.FilePath.Should().Be(validPath);
    }

    [Fact(DisplayName = nameof(AddCategory))]
    [Trait("Domain", "Video - Aggregate")]
    public void AddCategory()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var categoryIdExample = Guid.NewGuid();

        validVideo.AddCategory(categoryIdExample);

        validVideo.Categories.Should().HaveCount(1);
        validVideo.Categories[0].Should().Be(categoryIdExample);
    }

    [Fact(DisplayName = nameof(RemoveCategory))]
    [Trait("Domain", "Video - Aggregate")]
    public void RemoveCategory()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var categoryIdExample = Guid.NewGuid();
        var categoryIdExample2 = Guid.NewGuid();
        validVideo.AddCategory(categoryIdExample);
        validVideo.AddCategory(categoryIdExample2);

        validVideo.RemoveCategory(categoryIdExample);

        validVideo.Categories.Should().HaveCount(1);
        validVideo.Categories[0].Should().Be(categoryIdExample2);
    }

    [Fact(DisplayName = nameof(AddGenre))]
    [Trait("Domain", "Video - Aggregate")]
    public void AddGenre()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();

        validVideo.AddGenre(exampleId);

        validVideo.Genres.Should().HaveCount(1);
        validVideo.Genres[0].Should().Be(exampleId);
    }

    [Fact(DisplayName = nameof(RemoveGenre))]
    [Trait("Domain", "Video - Aggregate")]
    public void RemoveGenre()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        validVideo.AddGenre(exampleId);
        validVideo.AddGenre(exampleId2);

        validVideo.RemoveGenre(exampleId2);

        validVideo.Genres.Should().HaveCount(1);
        validVideo.Genres[0].Should().Be(exampleId);
    }

    [Fact(DisplayName = nameof(RemoveAllGenre))]
    [Trait("Domain", "Video - Aggregate")]
    public void RemoveAllGenre()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        validVideo.AddGenre(exampleId);
        validVideo.AddGenre(exampleId2);

        validVideo.RemoveAllGenres();

        validVideo.Genres.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(AddCastMember))]
    [Trait("Domain", "Video - Aggregate")]
    public void AddCastMember()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();

        validVideo.AddCastMember(exampleId);

        validVideo.CastMembers.Should().HaveCount(1);
        validVideo.CastMembers[0].Should().Be(exampleId);
    }

    [Fact(DisplayName = nameof(RemoveCastMember))]
    [Trait("Domain", "Video - Aggregate")]
    public void RemoveCastMember()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        validVideo.AddCastMember(exampleId);
        validVideo.AddCastMember(exampleId2);

        validVideo.RemoveCastMember(exampleId2);

        validVideo.CastMembers.Should().HaveCount(1);
        validVideo.CastMembers[0].Should().Be(exampleId);
    }

    [Fact(DisplayName = nameof(RemoveAllCastMembers))]
    [Trait("Domain", "Video - Aggregate")]
    public void RemoveAllCastMembers()
    {
        var validVideo = VideoGenerator.GetValidVideo();
        var exampleId = Guid.NewGuid();
        var exampleId2 = Guid.NewGuid();
        validVideo.AddCastMember(exampleId);
        validVideo.AddCastMember(exampleId2);

        validVideo.RemoveAllCastMembers();

        validVideo.CastMembers.Should().HaveCount(0);
    }
}
