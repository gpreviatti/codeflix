using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Application.Messages;
using Application.UseCases.Category;
using Tests.Common.Generators.Dtos;

namespace Tests.Integration.Application.UseCases.Category;

public class CreateCategoryTest : CategoryTestFixture
{
    private readonly ICreateCategory _createCategory;

    public CreateCategoryTest()
    {
        _createCategory = new CreateCategory(categoryRepository, unitOfWork);
    }

    [Fact]
    [Trait("Integration/Application", "Create - Use Cases")]
    public async Task Create()
    {
        var input = CreateCategoryInputGenerator.GetCategoryInput();

        var output = await _createCategory.Handle(input, CancellationToken.None);

        output.GetType().Should().Be<BaseResponse<CategoryOutput>>().And.NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Is_Active.Should().BeTrue();
        output.Data.Created_At.Should().NotBe(default);
    }
}
