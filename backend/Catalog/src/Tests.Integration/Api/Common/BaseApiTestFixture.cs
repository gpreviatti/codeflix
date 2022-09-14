namespace Tests.Integration.Api.Common;

public class BaseApiTestFixture : BaseFixture
{
    public ApiClient apiClient;
    public CustomWebApplicationFactory<Program> webApplicationFactory;

    public BaseApiTestFixture()
    {
        webApplicationFactory = new CustomWebApplicationFactory<Program>();

        var httpClient = webApplicationFactory.CreateClient();
        apiClient = new ApiClient(httpClient);
    }
}
