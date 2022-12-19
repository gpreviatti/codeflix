using Application.Exceptions;
using Domain.Repository;
using Domain.SeedWork.SearchableRepository;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Data.Repositories;
public class CastMemberRepositoryTest : BaseFixture
{
    private readonly ICastMemberRepository _repository;

    public CastMemberRepositoryTest()
    {
        _repository = new CastMemberRepository(dbContext);
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Insert()
    {
        var castMemberExample = CastMemberGenerator.GetFakerCastMember();


        await _repository.Insert(castMemberExample, CancellationToken.None);
        dbContext.SaveChanges();


        var castMemberFromDb = dbContext.CastMembers.FirstOrDefault(c => c.Id == castMemberExample.Id);
        castMemberFromDb.Should().NotBeNull();
        castMemberFromDb!.Name.Should().Be(castMemberExample.Name);
        castMemberFromDb.Type.Should().Be(castMemberExample.Type);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Get()
    {
        var castMemberExampleList = CastMemberGenerator.GetExampleCastMembersList();
        var castMemberExample = castMemberExampleList[3];
        await dbContext.AddRangeAsync(castMemberExampleList);
        await dbContext.SaveChangesAsync();


        var itemFromRepository = await _repository.Get(
            castMemberExample.Id,
            CancellationToken.None
        );


        itemFromRepository.Should().NotBeNull();
        itemFromRepository!.Name.Should().Be(castMemberExample.Name);
        itemFromRepository.Type.Should().Be(castMemberExample.Type);
    }

    [Fact(DisplayName = nameof(GetThrowsWhenNotFound))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task GetThrowsWhenNotFound()
    {
        var randomGuid = Guid.NewGuid();


        var action = async () => await _repository.Get(
            randomGuid,
            CancellationToken.None
        );


        await action.Should().ThrowAsync<NullReferenceException>()
            .WithMessage($"CastMember '{randomGuid}' not found.");
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Delete()
    {
        var castMemberExampleList = CastMemberGenerator.GetExampleCastMembersList(5);
        var castMemberExample = castMemberExampleList[3];
        await dbContext.AddRangeAsync(castMemberExampleList);
        await dbContext.SaveChangesAsync();


        await _repository.Delete(
            castMemberExample, CancellationToken.None
        );
        await dbContext.SaveChangesAsync();

        
        dbContext.CastMembers.Should().NotContain(castMemberExample);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Update()
    {
        var castMemberExampleList = CastMemberGenerator.GetExampleCastMembersList(5);
        var castMemberExample = castMemberExampleList[3];
        var newName = CastMemberGenerator.GetCastMamemberName();
        var newType = CastMemberGenerator.GetRandomCastMemberType();
        await dbContext.AddRangeAsync(castMemberExampleList);
        await dbContext.SaveChangesAsync();


        castMemberExample.Update(newName, newType);
        await _repository.Update(castMemberExample, CancellationToken.None);
        await dbContext.SaveChangesAsync();


        var castMemberDb = await dbContext.CastMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
        castMemberDb.Should().NotBeNull();
        castMemberDb!.Name.Should().Be(newName);
        castMemberDb.Type.Should().Be(newType);
    }

    [Fact(DisplayName = nameof(Search))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Search()
    {
        var exampleList = CastMemberGenerator.GetExampleCastMembersList(10);
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();

        var searchResult = await _repository.Search(
            new SearchInput(1, 20, "", "", SearchOrder.Asc),
            CancellationToken.None
        );

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(1);
        searchResult.PerPage.Should().Be(20);
        searchResult.Items.Should().HaveCount(20);
    }

    [Theory(DisplayName = nameof(OrderedSearch))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task OrderedSearch(
        string orderBy,
        string order
    )
    {
        var exampleList = CastMemberGenerator.GetExampleCastMembersList(5);
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;

        var searchResult = await _repository.Search(
            new SearchInput(1, 10, "", orderBy, inputOrder),
            CancellationToken.None
        );

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(1);
        searchResult.PerPage.Should().Be(10);
        searchResult.Items.Should().HaveCount(10);
    }
}
