using Application.Dtos.Category;
using Application.Exceptions;
using Domain.Entity;
using Domain.Excpetions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases.UpdateCategory;

public class UpdateCategoryTest : UpdateCategoryTestFixture
{
    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategory(
        Category category,
        UpdateCategoryInput input
    )
    {
        _repositoryMock.Setup(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(category);

        var output = await _updateCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        _repositoryMock.Verify(x => x.Get(
            category.Id,It.IsAny<CancellationToken>()), 
            Times.Once
        );

        _repositoryMock.Verify(x => x.Update(
            category,It.IsAny<CancellationToken>()), 
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryWithoutProvidingIsActive(
        Category exampleCategory,
        UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description
        );
        _repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var output = await _updateCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    
        _repositoryMock.Verify(
            x => x.Get(input.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _repositoryMock.Verify(
            x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), 
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }


    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryOnlyName(
        Category exampleCategory,
        UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name
        );
        _repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var output = await _updateCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        
        _repositoryMock.Verify(
            x => x.Get(input.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );
        
        _repositoryMock.Verify(
            x => x.Update( exampleCategory, It.IsAny<CancellationToken>()) , 
            Times.Once
        );

        _unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var input = GetValidInput();
        _repositoryMock.Setup(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));

        var task = async () => await _updateCategory.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        _repositoryMock.Verify(
            x => x.Get(input.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task ThrowWhenCantUpdateCategory(
        UpdateCategoryInput input,
        string expectedExceptionMessage
    )
    {
        var exampleCategory = GetValidCategory();
        input.Id = exampleCategory.Id;

        _repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var task = async () => await _updateCategory.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);

        _repositoryMock.Verify(
            x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
