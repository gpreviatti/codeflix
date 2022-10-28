using Application.UseCases.Category;
using Application.UseCases.Genre;
using Domain.Repository;
using Infra.Data;
using Infra.Data.Repositories;
using MediatR;

namespace Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        // adicionar referencia de qualquer use case que implemente o handler do mediatR
        services.AddMediatR(typeof(CreateCategory));
        services.AddMediatR(typeof(CreateGenre));
        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
