using Application.Dtos.Category;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Tests.Common.Generators.Entities;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryTest : CategoryBaseFixture
{
    protected IGetCategory _getCategory;

    public GetCategoryTest()
    {
        _getCategory = new CategoryUseCase.GetCategory(_repositoryMock.Object);
    }

    [Fact]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var category = CategoryGenerator.GetCategory();
        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

         var input = new GetCategoryInput(category.Id);

        var output = await _getCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().Be(category.Id);
        output.Description.Should().Be(category.Description);
        output.Is_Active.Should().Be(category.IsActive);
        output.Created_At.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    [Trait("Application", "NotFoundExceptionWhenCategoryDoesntExist - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var guid = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{guid} not found"));

        var input = new GetCategoryInput(guid);

        var task = async () => await _getCategory.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        _repositoryMock.Verify(
            r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }
}
