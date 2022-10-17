using Domain.Entity;

namespace Tests.Common.Generators.Entities;

public class GenreGenerator : CommonGenerator
{

    public static string GetValidName() => GetFaker().Commerce.Categories(1)[0];

    public static Genre GetExampleGenre(
    bool isActive = true,
    List<Guid>? categoriesIdsList = null
    )
    {
        var genre = new Genre(GetValidName(), isActive);
        if (categoriesIdsList is not null)
            foreach (var categoryId in categoriesIdsList)
                genre.AddCategory(categoryId);

        return genre;
    }
}
