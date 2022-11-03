using Application.Dtos.Genre;

namespace Tests.Common.Generators.Dtos;
public class UpdateGenreInputGenerator : CommonGenerator
{
    public static UpdateGenreInput GetGenre(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetFaker().Commerce.ProductName(),
        GetRandomBoolean()
    );
}
