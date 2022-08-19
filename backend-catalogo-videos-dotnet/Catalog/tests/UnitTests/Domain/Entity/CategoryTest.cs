using Xunit;
using Domain.Entity;

namespace Unit.Domain.Entity;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new Category("category name", "category description");
        var datetimeBefore = DateTime.Now;

        var category = new Category(validData.Name, validData.Description);

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default, validData.Id);
        Assert.NotEqual(default, validData.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < DateTime.Now);
        Assert.False(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new Category("category name", "category description");
        var datetimeBefore = DateTime.Now;

        var category = new Category(validData.Name, validData.Description, isActive);

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default, validData.Id);
        Assert.NotEqual(default, validData.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < DateTime.Now);
        Assert.Equal(isActive, category.IsActive);
    }
}
