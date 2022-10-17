using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using DomainEntity = Domain.Entity;
using Domain.Excpetions;
using Tests.Common.Generators;
using Tests.Common.Generators.Dtos;
using CategoryUseCases = Application.UseCases.Category;

namespace Tests.Unit.Application.UseCases.Category;

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
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Is_Active.Should().Be(input.Is_Active);
        output.Data.Created_At.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()),
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
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be("");
        output.Data.Is_Active.Should().Be(true);
        output.Data.Created_At.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()),
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
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<BaseResponse<CategoryOutput>>();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Is_Active.Should().Be(true);
        output.Data.Created_At.Should().NotBe(default);

        _repositoryMock.Verify(
            r => r.Insert(It.IsAny<DomainEntity.Category>(), It.IsAny<CancellationToken>()),
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
