using Domain.Entity;

namespace Tests.Common.Generators.Entities;

public class CategoryGenerator : CommonGenerator
{
    public static Category GetCategory() => FakerCategory();

    public static string GetProductName() => GetFaker().Commerce.ProductName();

    public static string GetProductDescription() => GetFaker().Commerce.ProductDescription();

    public static IList<Category> GetCategories(int count = 10)
    {
        var categories = new List<Category>();

        for (var i = 0; i < count; i++)
            categories.Add(FakerCategory());
        
        return categories;
    }

    private static Category FakerCategory() => new(
        GetProductName(),
        GetProductDescription()
    );
}
