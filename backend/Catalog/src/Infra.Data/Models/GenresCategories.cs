using Domain.Entity;

namespace Infra.Data.Models;

public class GenresCategories
{
    public GenresCategories(
        Guid categoryId,
        Guid genreId
    )
    {
        CategoryId = categoryId;
        GenreId = genreId;
    }

    public Guid CategoryId { get; set; }
    public Guid GenreId { get; set; }
    public Category? Category { get; set; }
    public Genre? Genre { get; set; }
}