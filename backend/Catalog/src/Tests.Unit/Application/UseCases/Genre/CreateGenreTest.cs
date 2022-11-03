using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Excpetions;
using Tests.Common.Generators.Dtos;
using DomainEntity = Domain.Entity;
using GenreUseCases = Application.UseCases.Genre;

namespace Tests.Unit.Application.UseCases.Genre;

public class CreateGenreTest : GenreBaseFixture
{
    private readonly ICreateGenre _createGenre;

    public CreateGenreTest()
    {
        _createGenre = new GenreUseCases.CreateGenre(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object,
            _categoryRepositoryMock.Object
        );
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task Create()
    {
        var input = CreateGenreInputGenerator.GetExampleInput();

        var datetimeBefore = DateTime.Now;
        var output = await _createGenre.Handle(input, CancellationToken.None);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        
        _repositoryMock.Verify(x => x.Insert(
            It.IsAny<DomainEntity.Genre>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.Should().NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Is_Active.Should().Be(input.Is_Active);
        output.Data.Categories.Should().HaveCount(0);
        output.Data.Created_At.Should().NotBeSameDateAs(default);
        (output.Data.Created_At >= datetimeBefore).Should().BeTrue();
        (output.Data.Created_At <= datetimeAfter).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(CreateWithRelatedCategories))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task CreateWithRelatedCategories()
    {
        var input = CreateGenreInputGenerator.GetExampleInputWithCategories();
        _categoryRepositoryMock.Setup(
            x => x.GetIdsListByIds(
                It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(input.Categories_Ids!);

        var output = await _createGenre.Handle(input, CancellationToken.None);

        _repositoryMock.Verify(x => x.Insert(
            It.IsAny<DomainEntity.Genre>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.Should().NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Is_Active.Should().Be(input.Is_Active);
        output.Data.Categories.Should().HaveCount(input.Categories_Ids?.Count ?? 0);
        input.Categories_Ids?.ForEach(id =>
            output.Data.Categories.Should().Contain(relation => relation.Id == id)
        );
        output.Data.Created_At.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateThrowWhenRelatedCategoryNotFound))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task CreateThrowWhenRelatedCategoryNotFound()
    {
        var input = CreateGenreInputGenerator.GetExampleInputWithCategories();
        var exampleGuid = input.Categories_Ids![^1];
        _categoryRepositoryMock.Setup(
            x => x.GetIdsListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            input.Categories_Ids
                .FindAll(x => x != exampleGuid)
        );

        var action = async () => await _createGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related category id (or ids) not found: {exampleGuid}");
        _categoryRepositoryMock.Verify(x =>
            x.GetIdsListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenNameIsInvalid))]
    [Trait("Application", "CreateGenre - Use Cases")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task ThrowWhenNameIsInvalid(string name)
    {
        var input = CreateGenreInputGenerator.GetExampleInput(name);

        var action = async () => await _createGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage($"Name should not be empty or null");
    }
}
