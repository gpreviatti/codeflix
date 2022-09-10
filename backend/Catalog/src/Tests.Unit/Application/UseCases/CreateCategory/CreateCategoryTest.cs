﻿using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Domain.Entity;
using Domain.Excpetions;
using Tests.Common.Generators;
using Tests.Common.Generators.Dtos;
using CategoryUseCases = Application.UseCases.Category;

namespace Unit.Application.UseCases.CreateCategory;

public class CreateCategoryTest : CategoryBaseFixture
{
    private readonly ICreateCategory _createCategory;

    public CreateCategoryTest()
    {
        _createCategory = new CategoryUseCases.CreateCategory(
            _repositoryMock.Object, _unitOfWorkMock.Object
        );
    }

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var input = CreateCategoryInputGenerator.GetValidCategoryInput();

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

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithOnlyName()
    {
        var name = CommonGenerator.GetFaker().Commerce.ProductName();
        var input = new CreateCategoryInput(name, "");

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

    [Fact]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithNameAndDescription()
    {
        var input = CreateCategoryInputGenerator.GetValidCategoryInput();

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

    [Theory]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryInputGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateCategoryInputGenerator)
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
