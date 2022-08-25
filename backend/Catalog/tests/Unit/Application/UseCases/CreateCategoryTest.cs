using Application.Dtos.Category;
using Domain.Entity;
using Domain.Excpetions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Unit.Application.UseCases;

public class CreateCategoryTest : CreateCategoryTestFixture
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {   
        var input = GetValidCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.GetType().Should().Be<CreateCategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(default);

        _respoitoryMock.Verify(
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
        output.GetType().Should().Be<CreateCategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBe(default);

        _respoitoryMock.Verify(
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
        output.GetType().Should().Be<CreateCategoryOutput>();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBe(default);

        _respoitoryMock.Verify(
            r => r.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );
        _unitOfWorkMock.Verify(
            u => u.Commit(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async Task ThrowWhenCantInstantiateAggregate(
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

    private static IEnumerable<object[]> GetInvalidInputs() 
    {
        var fixture = new CreateCategoryTestFixture();
        var inputList = new List<object[]>();

        // Nome não pode ser menor que 3 caracteres
        var inputShortName = fixture.GetValidCategoryInput();
        inputShortName.Name = inputShortName.Name[..2];
        inputList.Add(new object[] { "Name should be at least 3 characters", inputShortName } );

        // Nome não pode ser mais que 255 caracteres
        var inputTooLongName = fixture.GetValidCategoryInput();
        inputTooLongName.Name = fixture.Faker.Lorem.Letter(256);
        inputList.Add(new object[] { "Name should be less or equal 255 characters", inputTooLongName });

        // Nome não pode ser null
        var inputNullName = fixture.GetValidCategoryInput();
        inputNullName.Name = null!;
        inputList.Add(new object[] { "Name should not be empty or null", inputNullName });

        // Descricao não pode ser nula
        var inputNullDescription = fixture.GetValidCategoryInput();
        inputNullDescription.Description = null!;
        inputList.Add(new object[] { "Description should not be null", inputNullDescription });

        // Descricao não pode ser maior que 10000 caracters
        var inputTooLongDescription = fixture.GetValidCategoryInput();
        inputTooLongDescription.Description = fixture.Faker.Lorem.Letter(10001);
        inputList.Add(new object[] { "Description should be less or equal 10000 characters", inputTooLongDescription });

        return inputList;
    }
}
