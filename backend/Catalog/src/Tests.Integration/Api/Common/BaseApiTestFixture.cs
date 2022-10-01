using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.Integration.Api.Common;

public class BaseApiTestFixture
{
    public ApiClient apiClient;

    public BaseApiTestFixture()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Test"));

        var httpClient = application.CreateClient();
        apiClient = new ApiClient(httpClient);
    }
}
