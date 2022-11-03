using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.Integration.Api.Common;

public class BaseApiTestFixture
{
    public ApiClient apiClient;

    public BaseApiTestFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Environment.SetEnvironmentVariable("CONNECTION_STRING", "Server=localhost;Port=32006;Uid=root;Pwd=codeflix;Database=catalog_test;");

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Test"));

        var httpClient = application.CreateClient();
        apiClient = new ApiClient(httpClient);
    }
}
