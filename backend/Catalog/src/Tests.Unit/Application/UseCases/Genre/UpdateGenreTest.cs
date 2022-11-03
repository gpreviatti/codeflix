using Application.Dtos.Genre;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Domain.Excpetions;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using GenreUseCases = Application.UseCases.Genre;

namespace Tests.Unit.Application.UseCases.Genre;
public class UpdateGenreTest : GenreBaseFixture
{
    private readonly IUpdateGenre _updateGenre;

    public UpdateGenreTest()
    {
        _updateGenre = new GenreUseCases.UpdateGenre(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _categoryRepositoryMock.Object
        );
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenre()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();
        var newNameExample = GenreGenerator.GetValidName();
        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive
        );

        var output = await _updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(newIsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(0);
        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var exampleId = Guid.NewGuid();
        _repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre '{exampleId}' not found."
        ));

        var input = new UpdateGenreInput(
            exampleId,
            GenreGenerator.GetValidName(),
            true
        );

        var action = async () => await _updateGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found.");
    }

    [Theory(DisplayName = nameof(ThrowWhenNameIsInvalid))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task ThrowWhenNameIsInvalid(string? name)
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();

        var newIsActive = !exampleGenre.IsActive;
        
        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);
        
        var input = new UpdateGenreInput(
            exampleGenre.Id,
            name!,
            newIsActive
        );

        var action = async ()
            => await _updateGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage($"Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateGenreOnlyName))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateGenreOnlyName(bool isActive)
    {
        var exampleGenre = GenreGenerator.GetExampleGenre(isActive: isActive);
        var newNameExample = GenreGenerator.GetValidName();
        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);
        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample
        );

        var output = await _updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(isActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(0);

        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(UpdateGenreAddingCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreAddingCategoriesIds()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();
        var exampleCategoriesIdsList = GenreGenerator.GetRandomIdsList();
        var newNameExample = GenreGenerator.GetValidName();
        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        _categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategoriesIdsList);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleCategoriesIdsList
        );

        var output = await _updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(newIsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
        exampleCategoriesIdsList.ForEach(
            expectedId => output.Data.Categories.Should().Contain(relation => relation.Id == expectedId)
        );
        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(UpdateGenreReplacingCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreReplacingCategoriesIds()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre(
            categoriesIdsList: GenreGenerator.GetRandomIdsList()
        );
        var exampleCategoriesIdsList = GenreGenerator.GetRandomIdsList();
        var newNameExample = GenreGenerator.GetValidName();
        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        _categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategoriesIdsList);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleCategoriesIdsList
        );

        var output = await _updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(newIsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
        exampleCategoriesIdsList.ForEach(
            expectedId => output.Data.Categories.Should().Contain(relation => relation.Id == expectedId)
        );

        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre(
            categoriesIdsList: GenreGenerator.GetRandomIdsList()
        );
        var exampleNewCategoriesIdsList = GenreGenerator.GetRandomIdsList(10);
        var listReturnedByCategoryRepository =
            exampleNewCategoriesIdsList
                .GetRange(0, exampleNewCategoriesIdsList.Count - 2);

        var IdsNotReturnedByCategoryRepository =
            exampleNewCategoriesIdsList
                .GetRange(exampleNewCategoriesIdsList.Count - 2, 2);
        
        var newNameExample = GenreGenerator.GetValidName();
        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);
        
        _categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(listReturnedByCategoryRepository);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            exampleNewCategoriesIdsList
        );

        var action = async () => await _updateGenre.Handle(input, CancellationToken.None);

        var notFoundIdsAsString = String.Join(
            ", ",
            IdsNotReturnedByCategoryRepository
        );
        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage(
            $"Related category id (or ids) not found: {notFoundIdsAsString}"
        );
    }

    [Fact(DisplayName = nameof(UpdateGenreWithoutCategoriesIds))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreWithoutCategoriesIds()
    {
        var exampleCategoriesIdsList = GenreGenerator.GetRandomIdsList();
        var exampleGenre = GenreGenerator.GetExampleGenre(
            categoriesIdsList: exampleCategoriesIdsList
        );
        var newNameExample = GenreGenerator.GetValidName();

        var newIsActive = !exampleGenre.IsActive;
        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive
        );

        var output = await _updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(newIsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
        exampleCategoriesIdsList.ForEach(
            expectedId => output.Data.Categories.Should().Contain(relation => relation.Id == expectedId)
        );
        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(UpdateGenreWithEmptyCategoriesIdsList))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task UpdateGenreWithEmptyCategoriesIdsList()
    {
        var exampleCategoriesIdsList = GenreGenerator.GetRandomIdsList();
        var exampleGenre = GenreGenerator.GetExampleGenre(categoriesIdsList: exampleCategoriesIdsList);

        var newNameExample = GenreGenerator.GetValidName();

        var newIsActive = !exampleGenre.IsActive;

        _repositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);

        var input = new UpdateGenreInput(
            exampleGenre.Id,
            newNameExample,
            newIsActive,
            new List<Guid>()
        );


        var output = await _updateGenre.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleGenre.Id);
        output.Data.Name.Should().Be(newNameExample);
        output.Data.Is_Active.Should().Be(newIsActive);
        output.Data.Created_At.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Data.Categories.Should().HaveCount(0);

        _repositoryMock.Verify(
            x => x.Update(
                It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
