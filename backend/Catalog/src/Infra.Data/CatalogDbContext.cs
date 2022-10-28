using Domain.Entity;
using Infra.Data.Configurations;
using Infra.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data;
public class CatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<GenresCategories> GenresCategories => Set<GenresCategories>();

    //public DbSet<CastMember> CastMembers => Set<CastMember>();

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) 
    {
        // Se tem uma migração nova faz no startup do projeto
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new GenreConfiguration());
        builder.ApplyConfiguration(new GenreCategoriesConfiguration());
    }
}
