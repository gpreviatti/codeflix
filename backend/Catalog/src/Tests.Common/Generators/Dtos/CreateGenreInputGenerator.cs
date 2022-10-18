using Application.Dtos.Genre;

namespace Tests.Common.Generators.Dtos;

public class CreateGenreInputGenerator : CommonGenerator
{
    public static string GetValidGenreName() => GetFaker().Commerce.Categories(1)[0];

    public static CreateGenreInput GetExampleInput() => new(
        GetValidGenreName(),
        GetRandomBoolean()
    );
    
    public static CreateGenreInput GetExampleInput(string? name) => new(
        name!,
        GetRandomBoolean()
    );

    public static CreateGenreInput GetExampleInputWithCategories()
    {
        var numberOfCategoriesIds = (new Random()).Next(1, 10);
        
        var categoriesIds = Enumerable
            .Range(1, numberOfCategoriesIds)
            .Select(_ => Guid.NewGuid())
            .ToList();
        
        return new(
            GetValidGenreName(),
            GetRandomBoolean(),
            categoriesIds
        );
    }
}
