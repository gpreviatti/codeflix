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

    public List<Genre> GetExampleGenresList(int count = 10) => Enumerable.Range(1, count)
        .Select(_ => {
            var genre = new Genre(
                GetValidName(),
                GetRandomBoolean()
            );
            GetRandomIdsList().ForEach(genre.AddCategory);
            return genre;
        }).ToList();

    public List<Guid> GetRandomIdsList(int? count = null) => Enumerable
        .Range(1, count ?? (new Random()).Next(1, 10))
        .Select(_ => Guid.NewGuid())
        .ToList();
}
