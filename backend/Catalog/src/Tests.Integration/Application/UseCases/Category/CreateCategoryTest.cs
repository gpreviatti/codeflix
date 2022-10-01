using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.UseCases.Category;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Application.UseCases.Category;

public class CreateCategoryTest : CategoryTestFixture
{
    private readonly ICreateCategory _createCategory;

    public CreateCategoryTest()
    {
        _createCategory = new CreateCategory(_categoryRepository, _unitOfWork);
    }

    [Fact]
    [Trait("Integration/Application", "Create - Use Cases")]
    public async Task Create()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<CategoryOutput>().And.NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Is_Active.Should().BeTrue();
        output.Created_At.Should().NotBe(default);
    }
}
