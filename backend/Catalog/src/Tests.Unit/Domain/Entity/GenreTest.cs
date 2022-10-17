using Domain.Entity;
using Domain.Excpetions;
using Tests.Common.Generators.Entities;

namespace Tests.Unit.Domain.Entity;
public class GenreTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Instantiate()
    {
        var genreName = GenreGenerator.GetValidName();

        
        var datetimeBefore = DateTime.Now;
        var genre = new Genre(genreName);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        
        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
    }


    [Theory(DisplayName = nameof(InstantiateThrowWhenNameEmpty))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void InstantiateThrowWhenNameEmpty(string? name)
    {
        var action = () => new Genre(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var genreName = GenreGenerator.GetValidName();

        
        var datetimeBefore = DateTime.Now;
        var genre = new Genre(genreName, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        
        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Activate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Activate(bool isActive)
    {
        var genre = GenreGenerator.GetExampleGenre(isActive);
        var oldName = genre.Name;

        
        genre.Activate();

        
        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(oldName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Deactivate(bool isActive)
    {
        var genre = GenreGenerator.GetExampleGenre(isActive);
        var oldName = genre.Name;

        
        genre.Deactivate();

        
        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.IsActive.Should().BeFalse();
        genre.Name.Should().Be(oldName);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Update()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var newName = GenreGenerator.GetValidName();
        var oldIsActive = genre.IsActive;

        
        genre.Update(newName);

        
        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(newName);
        genre.IsActive.Should().Be(oldIsActive);
        genre.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(UpdateThrowWhenNameIsEmpty))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void UpdateThrowWhenNameIsEmpty(string? name)
    {
        var genre = GenreGenerator.GetExampleGenre();

        var action = () => genre.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(AddCategory))]
    [Trait("Domain", "Genre - Aggregates")]
    public void AddCategory()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var categoryGuid = Guid.NewGuid();

        
        genre.AddCategory(categoryGuid);

        
        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryGuid);
    }

    [Fact(DisplayName = nameof(AddTwoCategories))]
    [Trait("Domain", "Genre - Aggregates")]
    public void AddTwoCategories()
    {
        var genre = GenreGenerator.GetExampleGenre();
        var categoryGuid1 = Guid.NewGuid();
        var categoryGuid2 = Guid.NewGuid();

        
        genre.AddCategory(categoryGuid1);
        genre.AddCategory(categoryGuid2);


        genre.Categories.Should().HaveCount(2);
        genre.Categories.Should().Contain(categoryGuid1);
        genre.Categories.Should().Contain(categoryGuid2);
    }

    [Fact(DisplayName = nameof(RemoveCategory))]
    [Trait("Domain", "Genre - Aggregates")]
    public void RemoveCategory()
    {
        var exampleGuid = Guid.NewGuid();
        var genres = new List<Guid>()
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            exampleGuid,
            Guid.NewGuid(),
            Guid.NewGuid()
        };


        var genre = GenreGenerator.GetExampleGenre(true, genres);
        genre.RemoveCategory(exampleGuid);


        genre.Categories.Should().HaveCount(genres.Count - 1);
        genre.Categories.Should().NotContain(exampleGuid);
    }

    [Fact(DisplayName = nameof(RemoveAllCategories))]
    [Trait("Domain", "Genre - Aggregates")]
    public void RemoveAllCategories()
    {
        var genres = new List<Guid>()
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };


        var genre = GenreGenerator.GetExampleGenre(true, genres);
        genre.RemoveAllCategories();


        genre.Categories.Should().HaveCount(0);
    }
}
