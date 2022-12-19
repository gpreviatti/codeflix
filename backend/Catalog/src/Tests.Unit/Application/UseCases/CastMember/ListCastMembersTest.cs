using Application.Dtos.CastMember;
using Application.Interfaces.UseCases;
using Domain.SeedWork.SearchableRepository;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using UseCase = Application.UseCases.CastMember;

namespace Tests.Unit.Application.UseCases.CastMember;
public class ListCastMembersTest : CastMemberBaseFixture
{
    private readonly IListCastMembers _useCase;

    public ListCastMembersTest()
    {
        _useCase = new UseCase.ListCastMembers(
            _repositoryMock.Object
        );
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCastMembers - Use Cases")]
    public async Task List()
    {
        var castMembersListExample = CastMemberGenerator.GetExampleCastMembersList(3);
        var repositorySearchOutput = new SearchOutput<DomainEntity.CastMember>(
            1, 10, castMembersListExample.Count, It.IsAny<int>(),
            (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample
        );
        _repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositorySearchOutput);
        var input = new ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);

        var output = await _useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Meta.Page.Should().Be(repositorySearchOutput.CurrentPage);
        output.Meta.Per_Page.Should().Be(repositorySearchOutput.PerPage);
        output.Meta.Total.Should().Be(repositorySearchOutput.Total);
        output.Data.ToList().ForEach(outputItem =>
        {
            var example = castMembersListExample.Find(x => x.Id == outputItem.Id);
            example.Should().NotBeNull();
            outputItem.Name.Should().Be(example!.Name);
            outputItem.Type.Should().Be(example!.Type);
        });
        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(x => (
                x.Page == input.Page
                && x.PerPage == input.Per_Page
                && x.Search == input.Search
                && x.Order == input.Dir
                && x.OrderBy == input.Sort
            )),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(RetursEmptyWhenIsEmpty))]
    [Trait("Application", "ListCastMembers - Use Cases")]
    public async Task RetursEmptyWhenIsEmpty()
    {
        var castMembersListExample = new List<DomainEntity.CastMember>();
        var repositorySearchOutput = new SearchOutput<DomainEntity.CastMember>(
            1, 10, castMembersListExample.Count, It.IsAny<int>(),
            (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample
        );
        _repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositorySearchOutput);
        var input = new ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);


        var output = await _useCase.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Meta.Page.Should().Be(repositorySearchOutput.CurrentPage);
        output.Meta.Per_Page.Should().Be(repositorySearchOutput.PerPage);
        output.Meta.Total.Should().Be(repositorySearchOutput.Total);
        output.Data.Should().HaveCount(castMembersListExample.Count);
        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(x => (
                x.Page == input.Page
                && x.PerPage == input.Per_Page
                && x.Search == input.Search
                && x.Order == input.Dir
                && x.OrderBy == input.Sort
            )),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
