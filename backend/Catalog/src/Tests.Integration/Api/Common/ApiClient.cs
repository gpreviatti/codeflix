

using System.Text;
using System.Text.Json;

namespace Tests.Integration.Api.Common;
public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string resourceUrl,
        object payload
    ) where TOutput : class
    {
        var streamContent = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            resourceUrl,
            streamContent
        );

        var outputString = await response.Content.ReadAsStringAsync();

        TOutput? output = null;
        if(!string.IsNullOrEmpty(outputString))
        {
            output = JsonSerializer.Deserialize<TOutput>(
                outputString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        return (response, output);
    }
}
