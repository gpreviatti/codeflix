using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Extensions;
using Domain.Repository;
using Tests.Common.Generators.Dtos;
using DomainEntity = Domain.Entity;
using UseCase = Application.UseCases.Video;

namespace Tests.Unit.Application.UseCases.Video;
public class CreateVideoTest : VideoBaseFixture
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private readonly Mock<IGenreRepository> _genreRepositoryMock = new();
    private readonly Mock<ICastMemberRepository> _castMemberRepositoryMock = new();

    private readonly ICreateVideo _useCase;
    public CreateVideoTest()
    {
        _useCase = new UseCase.CreateVideo(
            _videoRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _genreRepositoryMock.Object,
            _castMemberRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _storageServiceMock.Object
        );
    }

    [Fact(DisplayName = nameof(CreateVideo))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideo()
    {
        var input = CreateVideoInputGenerator.CreateValidInput();


        var output = await _useCase.Handle(input, CancellationToken.None);


        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
    }

    [Fact(DisplayName = nameof(CreateVideoWithThumb))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithThumb()
    {
        var expectedThumbName = "thumb.jpg";

        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedThumbName);

        var input = CreateVideoInputGenerator.CreateValidInput(thumb: CreateVideoInputGenerator.GetValidImageFileInput());


        var output = await _useCase.Handle(input, CancellationToken.None);


        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();

        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.ThumbFileUrl.Should().Be(expectedThumbName);
    }

    [Fact(DisplayName = nameof(CreateVideoWithBanner))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithBanner()
    {
        var expectedThumbHalfName = "thumbhalf.jpg";
        
        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedThumbHalfName);
        
        var input = CreateVideoInputGenerator.CreateValidInput(thumbHalf: CreateVideoInputGenerator.GetValidImageFileInput());

        var output = await _useCase.Handle(input, CancellationToken.None);

        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();
        
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.ThumbHalfFileUrl.Should().Be(expectedThumbHalfName);
    }

    [Fact(DisplayName = nameof(CreateVideoWithThumbHalf))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithThumbHalf()
    {
        var expectedBannerName = "banner.jpg";
        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedBannerName);
        

        var input = CreateVideoInputGenerator.CreateValidInput(banner: CreateVideoInputGenerator.GetValidImageFileInput());

        var output = await _useCase.Handle(input, CancellationToken.None);

        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();
        
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.BannerFileUrl.Should().Be(expectedBannerName);
    }

    [Fact(DisplayName = nameof(CreateVideoWithAllImages))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithAllImages()
    {
        var expectedThumbHalfName = "thumbhalf.jpg";
        var expectedThumbName = "thumb.jpg";
        var expectedBannerName = "banner.jpg";

        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-banner.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedBannerName);
        
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-thumbhalf.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedThumbHalfName);
        
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-thumb.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedThumbName);
        
        var input = CreateVideoInputGenerator.CreateValidInputWithAllImages();

        var output = await _useCase.Handle(input, CancellationToken.None);

        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();

        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.ThumbHalfFileUrl.Should().Be(expectedThumbHalfName);
        output.ThumbFileUrl.Should().Be(expectedThumbName);
        output.BannerFileUrl.Should().Be(expectedBannerName);
    }

    [Fact(DisplayName = nameof(CreateVideoWithMedia))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithMedia()
    {
        var expectedMediaName = $"/storage/{CreateVideoInputGenerator.GetValidMediaPath()}";
        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedMediaName);

        var input = CreateVideoInputGenerator.CreateValidInput(media: CreateVideoInputGenerator.GetValidMediaFileInput());

        var output = await _useCase.Handle(input, CancellationToken.None);

        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );
        
        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();

        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.VideoFileUrl.Should().Be(expectedMediaName);
    }

    [Fact(DisplayName = nameof(CreateVideoWithTrailer))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithTrailer()
    {
        var expectedTrailerName = $"/storage/{CreateVideoInputGenerator.GetValidMediaPath()}";
        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedTrailerName);

        var input = CreateVideoInputGenerator.CreateValidInput(trailer: CreateVideoInputGenerator.GetValidMediaFileInput());

        
        var output = await _useCase.Handle(input, CancellationToken.None);


        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
                video =>
                    video.Title == input.Title &&
                    video.Published == input.Published &&
                    video.Description == input.Description &&
                    video.Duration == input.Duration &&
                    video.Rating == input.Rating &&
                    video.Id != Guid.Empty &&
                    video.YearLaunched == input.YearLaunched &&
                    video.Opened == input.Opened
            ),
            It.IsAny<CancellationToken>())
        );

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        _storageServiceMock.VerifyAll();

        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.TrailerFileUrl.Should().Be(expectedTrailerName);
    }

    [Fact(DisplayName = nameof(ThrowsExceptionInUploadErrorCases))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsExceptionInUploadErrorCases()
    {
        _storageServiceMock.Setup(x => x.Upload(
            It.IsAny<string>(),
            It.IsAny<Stream>(),
            It.IsAny<CancellationToken>()))
        .ThrowsAsync(new Exception("Something went wrong in upload"));
        var input = CreateVideoInputGenerator.CreateValidInputWithAllImages();


        var action = () => _useCase.Handle(input, CancellationToken.None);


        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Something went wrong in upload");
    }

    [Fact(DisplayName = nameof(ThrowsExceptionAndRollbackUploadInImageUploadErrorCases))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsExceptionAndRollbackUploadInImageUploadErrorCases()
    {
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-banner.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync("123-banner.jpg");
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-thumb.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync("123-thumb.jpg");
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith("-thumbhalf.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ThrowsAsync(new Exception("Something went wrong in upload"));

        var input = CreateVideoInputGenerator.CreateValidInputWithAllImages();

        
        var action = () => _useCase.Handle(input, CancellationToken.None);

        
        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Something went wrong in upload");
        _storageServiceMock.Verify(
            x => x.Delete(
                It.Is<string>(x => (x == "123-banner.jpg") || (x == "123-thumb.jpg")),
                It.IsAny<CancellationToken>()
            ), Times.Exactly(2));
    }

    [Fact(DisplayName = nameof(ThrowsExceptionAndRollbackMediaUploadInCommitErrorCases))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsExceptionAndRollbackMediaUploadInCommitErrorCases()
    {
        var input = CreateVideoInputGenerator.CreateValidInputWithAllMedias();

        var storageMediaPath = CreateVideoInputGenerator.GetValidMediaPath();
        var storageTrailerPath = CreateVideoInputGenerator.GetValidMediaPath();
        var storagePathList = new List<string>() { storageMediaPath, storageTrailerPath };
        
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith($"media.{input.Media!.Extension}")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(storageMediaPath);
        _storageServiceMock.Setup(x => x.Upload(
            It.Is<string>(x => x.EndsWith($"trailer.{input.Trailer!.Extension}")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(storageTrailerPath);
        
        _unitOfWorkMock.Setup(x => x.Commit(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wront with the commit"));


        var action = () => _useCase.Handle(input, CancellationToken.None);


        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Something went wront with the commit");
        _storageServiceMock.Verify(
            x => x.Delete(
                It.Is<string>(path => storagePathList.Contains(path)),
                It.IsAny<CancellationToken>()
            ), Times.Exactly(2));
        _storageServiceMock.Verify(
            x => x.Delete(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ), Times.Exactly(2));
    }

    [Fact(DisplayName = nameof(CreateVideoWithCategoriesIds))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithCategoriesIds()
    {
        var examplecategoriesIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();
        
        _categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(examplecategoriesIds);

        var input = CreateVideoInputGenerator.CreateValidInput(examplecategoriesIds);

        var output = await _useCase.Handle(input, CancellationToken.None);

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        List<Guid> outputItemCategoryIds = output.Categories
            .Select(categoryDto => categoryDto.Id).ToList();
        outputItemCategoryIds.Should().BeEquivalentTo(examplecategoriesIds);

        _videoRepositoryMock.Verify(x => x.Insert(
        It.Is<DomainEntity.Video>(
            video =>
                video.Title == input.Title &&
                video.Published == input.Published &&
                video.Description == input.Description &&
                video.Duration == input.Duration &&
                video.Rating == input.Rating &&
                video.Id != Guid.Empty &&
                video.YearLaunched == input.YearLaunched &&
                video.Opened == input.Opened &&
                video.Categories.All(categoryId => examplecategoriesIds.Contains(categoryId))
            ),
            It.IsAny<CancellationToken>())
        );
        _categoryRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowsWhenCategoryIdInvalid))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsWhenCategoryIdInvalid()
    {
        var examplecategoriesIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();

        var removedcategoryId = examplecategoriesIds[2];
        _categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(examplecategoriesIds.FindAll(x => x!= removedcategoryId).ToList().AsReadOnly());

        var input = CreateVideoInputGenerator.CreateValidInput(examplecategoriesIds);

        var action = () => _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related category id (or ids) not found: {removedcategoryId}.");
        _categoryRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(CreateVideoWithGenresIds))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithGenresIds()
    {
        var exampleIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();

        _genreRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleIds);

        var input = CreateVideoInputGenerator.CreateValidInput(genresIds: exampleIds);

        var output = await _useCase.Handle(input, CancellationToken.None);

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        var outputItemGenresIds = output.Genres
            .Select(dto => dto.Id).ToList();
        outputItemGenresIds.Should().BeEquivalentTo(exampleIds);

        _videoRepositoryMock.Verify(x => x.Insert(
        It.Is<DomainEntity.Video>(
            video =>
                video.Title == input.Title &&
                video.Published == input.Published &&
                video.Description == input.Description &&
                video.Duration == input.Duration &&
                video.Rating == input.Rating &&
                video.Id != Guid.Empty &&
                video.YearLaunched == input.YearLaunched &&
                video.Opened == input.Opened &&
                video.Genres.All(id => exampleIds.Contains(id))
            ),
            It.IsAny<CancellationToken>())
        );
        _genreRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowsWhenInvalidGenreId))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsWhenInvalidGenreId()
    {
        var exampleIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();
        var removedId = exampleIds[2];

        _genreRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleIds.FindAll(id => id != removedId));

        var input = CreateVideoInputGenerator.CreateValidInput(genresIds: exampleIds);

        var action = () => _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related genre id (or ids) not found: {removedId}.");

        _genreRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(CreateVideoWithCastMembersIds))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task CreateVideoWithCastMembersIds()
    {
        var exampleIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();
        
        _castMemberRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleIds);

        var input = CreateVideoInputGenerator.CreateValidInput(castMembersIds: exampleIds);

        var output = await _useCase.Handle(input, CancellationToken.None);

        _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating.ToStringSignal());
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        var outputItemCastMembersIds = output.CastMembers
            .Select(dto => dto.Id).ToList();
        outputItemCastMembersIds.Should().BeEquivalentTo(exampleIds);
        _videoRepositoryMock.Verify(x => x.Insert(
            It.Is<DomainEntity.Video>(
            video =>
                video.Title == input.Title &&
                video.Published == input.Published &&
                video.Description == input.Description &&
                video.Duration == input.Duration &&
                video.Rating == input.Rating &&
                video.Id != Guid.Empty &&
                video.YearLaunched == input.YearLaunched &&
                video.Opened == input.Opened &&
                video.CastMembers.All(id => exampleIds.Contains(id))
            ),
            It.IsAny<CancellationToken>())
        );
        _castMemberRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowsWhenInvalidCastMemberId))]
    [Trait("Application", "CreateVideo - Use Cases")]
    public async Task ThrowsWhenInvalidCastMemberId()
    {
        var exampleIds = Enumerable.Range(1, 5)
            .Select(_ => Guid.NewGuid()).ToList();
        var removedId = exampleIds[2];
        
        _castMemberRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleIds.FindAll(x => x != removedId));
        
        var input = CreateVideoInputGenerator.CreateValidInput(castMembersIds: exampleIds);

        var action = () => _useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related cast member id (or ids) not found: {removedId}.");
        _castMemberRepositoryMock.VerifyAll();
    }
}
