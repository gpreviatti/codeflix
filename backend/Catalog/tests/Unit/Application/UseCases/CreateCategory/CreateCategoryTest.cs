using Application.Dtos.Category;
using Domain.Entity;
using Domain.Excpetions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.CreateCategory;

public class CreateCategoryTest : CreateCategoryTestFixture
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var input = GetValidCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
        _unitOfWorkMock.Verify(
            u => u.Commit(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithOnlyName()
    {
        var input = new CreateCategoryInput(GetValidCategoryName(), "");

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
        _unitOfWorkMock.Verify(
            u => u.Commit(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact(DisplayName = nameof(CreateCategoryWithNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithNameAndDescription()
    {
        var input = new CreateCategoryInput(GetValidCategoryName(), GetValidCategoryDescription());

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
        _unitOfWorkMock.Verify(
            u => u.Commit(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCategory(
        string exceptionMessage,
        CreateCategoryInput input
    )
    {
        var action = async () => await _createCategory.Handle(input, CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
