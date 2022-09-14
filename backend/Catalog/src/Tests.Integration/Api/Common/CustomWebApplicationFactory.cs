using Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Api.Common;

public class CustomWebApplicationFactory<TStartup> :
    WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbOptions = services
                .FirstOrDefault(s => s.Equals(typeof(DbContextOptions<CatalogDbContext>)));

            if (dbOptions is not null) services.Remove(dbOptions);

            services.AddDbContext<CatalogDbContext>(
                options => options.UseInMemoryDatabase($"fc-db-integration-tests")
            );
        });

        base.ConfigureWebHost(builder);
    }
}
