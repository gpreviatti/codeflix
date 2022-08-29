using Application.Dtos.Category;
using Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.DeleteCategory;

public class DeleteCategoryTest : DeleteCategoryTestFixture
{
    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var categoryExample = GetValidCategory();
        _repositoryMock.Setup(x => x.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(categoryExample);

        var input = new DeleteCategoryInput(categoryExample.Id);

        await _deleteCategory.Handle(input, CancellationToken.None);

        _repositoryMock.Verify(x => x.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        _repositoryMock.Verify(x => x.Delete(
            categoryExample,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }


    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var exampleGuid = Guid.NewGuid();
        _repositoryMock.Setup(x => x.Get(
            exampleGuid,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Category '{exampleGuid}' not found")
        );
        var input = new DeleteCategoryInput(exampleGuid);

        var task = async () => await _deleteCategory.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        _repositoryMock.Verify(x => x.Get(
            exampleGuid,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
