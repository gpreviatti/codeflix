using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConnections(
        this IServiceCollection services
    )
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseInMemoryDatabase("fc-db-integration");
        });

        return services;
    }
}
