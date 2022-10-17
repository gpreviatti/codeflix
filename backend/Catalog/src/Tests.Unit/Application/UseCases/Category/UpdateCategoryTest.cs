using Application.Dtos.Category;
using Application.Exceptions;
using Application.Interfaces.UseCases;
using Application.Messages;
using DomainEntity = Domain.Entity;
using Domain.Excpetions;
using Tests.Common.Generators.Dtos;
using Tests.Common.Generators.Entities;
using UpdateCategoryUseCase = Application.UseCases.Category.UpdateCategory;

namespace Tests.Unit.Application.UseCases.Category;

public class UpdateCategoryTest : CategoryBaseFixture
{
    protected readonly IUpdateCategory _updateCategory;

    public UpdateCategoryTest()
    {
        _updateCategory = new UpdateCategoryUseCase(
            _repositoryMock.Object, _unitOfWorkMock.Object
        );
    }
    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryInputGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryInputGenerator)
    )]
    public async Task UpdateCategory(
        DomainEntity.Category category,
        UpdateCategoryInput input
    )
    {
        _repositoryMock.Setup(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(category);

        var output = await _updateCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Is_Active.Should().Be((bool)input.Is_Active!);

        _repositoryMock.Verify(x => x.Get(
            category.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );

        _repositoryMock.Verify(x => x.Update(
            category, It.IsAny<CancellationToken>()),
            Times.Once
        );

        _unitOfWorkMock.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryInputGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryInputGenerator)
    )]
    public async Task UpdateCategoryWithoutProvidingIsActive(
        DomainEntity.Category exampleCategory,
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
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Is_Active.Should().Be(exampleCategory.IsActive);

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


    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryInputGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryInputGenerator)
    )]
    public async Task UpdateCategoryOnlyName(
        DomainEntity.Category exampleCategory,
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
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.Is_Active.Should().Be(exampleCategory.IsActive);

        _repositoryMock.Verify(
            x => x.Get(input.Id, It.IsAny<CancellationToken>()),
            Times.Once
        );

        _repositoryMock.Verify(
            x => x.Update(exampleCategory, It.IsAny<CancellationToken>()),
            Times.Once
        );

        _unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var input = UpdateCategoryInputGenerator.GetCategory();
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

    [Theory]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryInputGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryInputGenerator)
    )]
    public async Task ThrowWhenCantUpdateCategory(
        UpdateCategoryInput input,
        string expectedExceptionMessage
    )
    {
        var exampleCategory = CategoryGenerator.GetCategory();
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
