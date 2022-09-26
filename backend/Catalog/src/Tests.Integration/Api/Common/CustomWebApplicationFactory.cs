using Infra.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Api.Common;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected async override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.Test.json")
            .Build();

        builder
            .UseEnvironment("Test")
            .UseConfiguration(configuration)
            .ConfigureServices(async services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetService<CatalogDbContext>();
                ArgumentNullException.ThrowIfNull(context);

                //await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            });

        base.ConfigureWebHost(builder);
    }
}
