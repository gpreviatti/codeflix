using Application.Dtos.Category;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.GetCategory;

public class GetCategoryTest : GetCategoryTestFixture
{
    [Fact(DisplayName = nameof(GetCategory))]
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
        output.GetType().Should().Be<GetCategoryOutput>();
        output.Id.Should().Be(category.Id);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().NotBe(default);

        _respoitoryMock.Verify(
            r => r.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }
}
