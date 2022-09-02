using Application.Dtos.Category;
using Domain.Entity;
using Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.UpdateCategory;

public class ListCategoriesTest : ListCategoriesTestFixture
{
    [Fact]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task List()
    {
        var categoriesExampleList = GetExampleCategoriesList();
        var input = GetExampleInput();
        var outputRepositorySearch = new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            new Random().Next(50, 200),
            categoriesExampleList
        );

        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        output.Items.ToList().ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch
                    .Items
                    .FirstOrDefault(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.IsActive.Should().Be(repositoryCategory!.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });

        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
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
        var input = GetExampleInput();
        var outputRepositorySearch = new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            0,
            new List<Category>().AsReadOnly()
        );

        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
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
        nameof(ListCategoriesTestDataGenerator.GetInputsWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(ListCategoriesTestDataGenerator)
    )]
    public async Task ListInputWithoutAllParameters(ListCategoriesInput input)
    {
        var categoriesExampleList = GetExampleCategoriesList();
        var outputRepositorySearch = new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            new Random().Next(50, 200),
            categoriesExampleList
        );
        _repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listCategories.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        output.Items.ToList().ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch
                .Items
                .FirstOrDefault(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory!.Description);
            outputItem.IsActive.Should().Be(repositoryCategory!.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });
        _repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
