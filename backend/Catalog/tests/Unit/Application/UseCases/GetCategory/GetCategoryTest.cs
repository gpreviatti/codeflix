using Application.Dtos.Category;
using Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryTest : GetCategoryTestFixture
{
    [Fact]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var category = GetValidCategory();
        _respoitoryMock
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var input = new GetCategoryInput(category.Id);

        var output = await _getCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().Be(category.Id);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().NotBe(default);

        _respoitoryMock.Verify(
            r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    [Trait("Application", "NotFoundExceptionWhenCategoryDoesntExist - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        var guid = Guid.NewGuid();
        _respoitoryMock
            .Setup(r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{guid} not found"));

        var input = new GetCategoryInput(guid);

        var task = async () => await _getCategory.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        _respoitoryMock.Verify(
            r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }
}
