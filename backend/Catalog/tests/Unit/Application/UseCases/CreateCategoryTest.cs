using Application.Dtos.Category;
using Application.Interfaces;
using Application.UseCases.Category;
using Domain.Entity;
using Domain.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var respoitoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new CreateCategory(respoitoryMock.Object, unitOfWorkMock.Object);
        var input = new CreateCategoryInput("Name", "Description", true);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be("Name");
        output.Description.Should().Be("Description");
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBe(default);

        respoitoryMock.Verify(
            r => r.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once()
        );
        unitOfWorkMock.Verify(
            u => u.Commit(It.IsAny<CancellationToken>()), 
            Times.Once()
        );
    }
}
