using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Domain.SeedWork.SearchableRepository;
using Tests.Common.Generators.Entities;
using DomainEntity = Domain.Entity;
using GenreUseCases = Application.UseCases.Genre;

namespace Tests.Unit.Application.UseCases.Genre;
public class ListGenresTest : GenreBaseFixture
{
    private readonly IListGenres _listGenres;

    public ListGenresTest()
    {
        _listGenres = new GenreUseCases.ListGenres(
            _repositoryMock.Object,
            _categoryRepositoryMock.Object
        );
    }

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListGenres()
    {
        var genresListExample = GenreGenerator.GetExampleGenresList();
        var input = new ListGenresInput(1, 3, "", "");

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            input.Page,
            input.Per_Page,
            new Random().Next(5, 10),
            10,
            genresListExample
        );

        _repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        
        var output = await _listGenres.Handle(input, CancellationToken.None);


        output.Meta.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Meta.Per_Page.Should().Be(outputRepositorySearch.PerPage);
        output.Meta.Total.Should().Be(outputRepositorySearch.Total);
        output.Data.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Data.ForEach(outputItem =>
        {
            var repositoryGenre = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            repositoryGenre.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryGenre!.Name);
            outputItem.Is_Active.Should().Be(repositoryGenre.IsActive);
            outputItem.Created_At.Should().Be(repositoryGenre!.CreatedAt);
            outputItem.Categories.Should()
                .HaveCount(repositoryGenre.Categories.Count);
            foreach (var expectedId in repositoryGenre.Categories)
                outputItem.Categories.Should().Contain(relation => relation.Id == expectedId);
        });

        _repositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.Per_Page
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        var expectedIds = genresListExample
            .SelectMany(genre => genre.Categories)
            .Distinct().ToList();

        _categoryRepositoryMock.Verify(
            x => x.GetListByIds(
                It.Is<List<Guid>>(parameterList =>
                    parameterList.All(id => expectedIds.Contains(id)
                    && parameterList.Count == expectedIds.Count
                )),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ListEmpty))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListEmpty()
    {
        var input = new ListGenresInput(1, 10, "", "");

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            input.Page,
            input.Per_Page,
            new Random().Next(50, 200),
            10,
            new List<DomainEntity.Genre>()
        );

        _repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var output = await _listGenres.Handle(input, CancellationToken.None);

        output.Meta.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.Meta.Per_Page.Should().Be(outputRepositorySearch.PerPage);
        output.Meta.Total.Should().Be(outputRepositorySearch.Total);
        output.Data.Should().HaveCount(outputRepositorySearch.Items.Count);
        
        _repositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.Per_Page
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _categoryRepositoryMock.Verify(
            x => x.GetListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Never
        );
    }

    [Fact(DisplayName = nameof(ListUsingDefaultInputValues))]
    [Trait("Application", "ListGenres - Use Cases")]
    public async Task ListUsingDefaultInputValues()
    {
        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            1,
            15,
            0,
            10,
            new List<DomainEntity.Genre>()
        );

        _repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);


        await _listGenres.Handle(new(), CancellationToken.None);


        _repositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == 1
                    && searchInput.PerPage == 15
                    && searchInput.Search == ""
                    && searchInput.OrderBy == ""
                    && searchInput.Order == SearchOrder.Asc
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _categoryRepositoryMock.Verify(
            x => x.GetListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Never
        );
    }
}
