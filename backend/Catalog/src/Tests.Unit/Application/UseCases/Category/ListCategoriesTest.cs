using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using DomainEntity = Domain.Entity;
using Domain.SeedWork.SearchableRepository;
using Tests.Common.Generators.Dtos;
using Tests.Common.Generators.Entities;
using CategoryUseCase = Application.UseCases.Category;

namespace Tests.Unit.Application.UseCases.Category;

public class ListCategoriesTest : CategoryBaseFixture
{
    protected readonly IListCategories _listCategories;

    public ListCategoriesTest()
    {
        _listCategories = new CategoryUseCase.ListCategories(_repositoryMock.Object);
    }

    [Fact]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task List()
    {
        var categoriesExampleList = CategoryGenerator.GetCategories().ToList();
        var input = ListCategoriesInputGenerator.GetInput();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            input.Page,
            input.Per_Page,
            new Random().Next(50, 200),
            10,
            categoriesExampleList
        );

        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>();
        output.Meta.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Meta.Per_Page.Should().Be(outputRepositorySearch.PerPage);
        output.Meta.Total.Should().Be(outputRepositorySearch.Total);
        output.Data.Should().HaveCount(outputRepositorySearch.Items.Count);

        output.Data.ToList().ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch
                    .Items
                    .FirstOrDefault(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.Is_Active.Should().Be(repositoryCategory!.IsActive);
            outputItem.Created_At.Should().Be(repositoryCategory!.CreatedAt);
        });

        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListOkWhenEmpty()
    {
        var input = ListCategoriesInputGenerator.GetInput();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            input.Page,
            input.Per_Page,
            0,
            10,
            new List<DomainEntity.Category>().AsReadOnly()
        );

        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<BasePaginatedResponse<List<CategoryOutput>>>();
        output.Meta.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Meta.Per_Page.Should().Be(outputRepositorySearch.PerPage);
        output.Meta.Total.Should().Be(0);
        output.Data.Should().HaveCount(0);

        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory]
    [Trait("Application", "ListCategories - Use Cases")]
    [MemberData(
        nameof(ListCategoriesInputGenerator.GetInputsWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(ListCategoriesInputGenerator)
    )]
    public async Task ListInputWithoutAllParameters(ListCategoriesInput input)
    {
        var categoriesExampleList = CategoryGenerator.GetCategories().ToList();
        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            input.Page,
            input.Per_Page,
            new Random().Next(50, 200),
            10,
            categoriesExampleList
        );

        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Meta.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Meta.Per_Page.Should().Be(outputRepositorySearch.PerPage);
        output.Meta.Total.Should().Be(outputRepositorySearch.Total);
        output.Data.Should().HaveCount(outputRepositorySearch.Items.Count);

        output.Data.ToList().ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch
                .Items
                .FirstOrDefault(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.Is_Active.Should().Be(repositoryCategory!.IsActive);
            outputItem.Created_At.Should().Be(repositoryCategory!.CreatedAt);
        });
        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.Per_Page
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
