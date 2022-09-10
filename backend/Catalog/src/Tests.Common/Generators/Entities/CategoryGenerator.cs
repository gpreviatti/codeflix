using Domain.Entity;

namespace Tests.Common.Generators.Entities;

public class CategoryGenerator : CommonGenerator
{
    public static Category GetCategory() => FakerCategory();

    public static string GetProductName() => GetFaker().Commerce.ProductName();

    public static string GetProductDescription() => GetFaker().Commerce.ProductDescription();

    public static IList<Category> GetCategories(int count)
    {
        var categories = new List<Category>();

        for (int i = 0; i < count; i++)
            categories.Add(FakerCategory());
        
        return categories;
    }

    private static Category FakerCategory() => new(
        GetProductName(),
        GetProductDescription()
    );
}
