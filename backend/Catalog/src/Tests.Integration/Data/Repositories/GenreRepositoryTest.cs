using Application.Exceptions;
using Domain.Repository;
using Domain.SeedWork;
using Infra.Data.Models;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Data.Repositories;
public class GenreRepositoryTest : BaseFixture
{
    private IGenreRepository repoistory;

    public GenreRepositoryTest()
    {
        repoistory = new GenreRepository(dbContext);
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Insert()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();
        
        var categoriesListExample = CategoryGenerator.GetCategories(3).ToList();

        categoriesListExample.ToList().ForEach(
            category => exampleGenre.AddCategory(category.Id)
        );

        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.SaveChangesAsync(CancellationToken.None);


        await repoistory.Insert(exampleGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);


        var dbGenre = await dbContext.Genres.FindAsync(exampleGenre.Id);
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        
        var genreCategoriesRelations = await dbContext
            .GenresCategories.Where(r => r.GenreId == exampleGenre.Id)
            .ToListAsync();

        genreCategoriesRelations.Should()
            .HaveCount(categoriesListExample.Count);

        genreCategoriesRelations.ForEach(relation => {
            var expectedCategory = categoriesListExample
                .FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Get()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();
        var categoriesListExample = CategoryGenerator.GetCategories(3);

        categoriesListExample.ToList().ForEach(
            category => exampleGenre.AddCategory(category.Id)
        );

        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        dbContext.SaveChanges();


        var genreFromRepository = await repoistory.Get(exampleGenre.Id, CancellationToken.None);


        genreFromRepository.Should().NotBeNull();
        genreFromRepository!.Name.Should().Be(exampleGenre.Name);
        genreFromRepository.IsActive.Should().Be(exampleGenre.IsActive);
        
        genreFromRepository.Categories.Should()
            .HaveCount(categoriesListExample.Count);
        
        foreach (var categoryId in genreFromRepository.Categories)
        {
            var expectedCategory = categoriesListExample
                .FirstOrDefault(x => x.Id == categoryId);
            expectedCategory.Should().NotBeNull();
        }
    }

    [Fact(DisplayName = nameof(GetThrowWhenNotFound))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task GetThrowWhenNotFound()
    {
        var exampleNotFoundGuid = Guid.NewGuid();

        var exampleGenre = GenreGenerator.GetExampleGenre();
        
        var categoriesListExample = CategoryGenerator.GetCategories(3);
        categoriesListExample.ToList().ForEach(
            category => exampleGenre.AddCategory(category.Id)
        );
        
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        
        await dbContext.Genres.AddAsync(exampleGenre);
        
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        dbContext.SaveChanges();


        var action = async () => await repoistory.Get(
            exampleNotFoundGuid,
            CancellationToken.None
        );


        await action.Should().ThrowAsync<NullReferenceException>()
            .WithMessage($"Genre '{exampleNotFoundGuid}' not found.");
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Delete()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();

        var categoriesListExample = CategoryGenerator.GetCategories(3);
        categoriesListExample.ToList().ForEach(
            category => exampleGenre.AddCategory(category.Id)
        );

        await dbContext.Categories.AddRangeAsync(categoriesListExample);

        await dbContext.Genres.AddAsync(exampleGenre);

        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        dbContext.SaveChanges();


        await repoistory.Delete(exampleGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync();


        var dbGenre = dbContext.Genres
            .AsNoTracking().FirstOrDefault(x => x.Id == exampleGenre.Id);
        dbGenre.Should().BeNull();

        var categoriesIdsList = await dbContext.GenresCategories
            .AsNoTracking().Where(x => x.GenreId == exampleGenre.Id)
            .Select(x => x.CategoryId)
            .ToListAsync();
        
        categoriesIdsList.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Update()
    {
        var exampleGenre = GenreGenerator.GetExampleGenre();

        var categoriesListExample = CategoryGenerator.GetCategories(3);
        categoriesListExample.ToList().ForEach(
            category => exampleGenre.AddCategory(category.Id)
        );

        await dbContext.Categories.AddRangeAsync(categoriesListExample);

        await dbContext.Genres.AddAsync(exampleGenre);

        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        dbContext.SaveChanges();


        exampleGenre.Update(exampleGenre.Name);
        if (exampleGenre.IsActive)
            exampleGenre.Deactivate();
        else
            exampleGenre.Activate();
        await repoistory.Update(
            exampleGenre,
            CancellationToken.None
        );
        await dbContext.SaveChangesAsync();


        var dbGenre = await dbContext.Genres.FindAsync(exampleGenre.Id);
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);

        var genreCategoriesRelations = await dbContext
            .GenresCategories.Where(r => r.GenreId == exampleGenre.Id)
            .ToListAsync();

        genreCategoriesRelations.Should()
            .HaveCount(categoriesListExample.Count);
        
        genreCategoriesRelations.ForEach(relation => {
            var expectedCategory = categoriesListExample
                .FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }


}
