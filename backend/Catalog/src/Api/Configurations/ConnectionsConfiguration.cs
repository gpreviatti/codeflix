using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConnections(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        return services;
    }
}
