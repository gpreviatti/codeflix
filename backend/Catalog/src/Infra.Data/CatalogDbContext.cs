using Domain.Entity;
using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data;
public class CatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    //public DbSet<Genre> Genres => Set<Genre>();
    //public DbSet<CastMember> CastMembers => Set<CastMember>();
    //public DbSet<GenresCategories> GenresCategories => Set<GenresCategories>();

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        //builder.ApplyConfiguration(new GenreConfiguration());
        //builder.ApplyConfiguration(new GenresCategoriesConfiguration());
    }
}
