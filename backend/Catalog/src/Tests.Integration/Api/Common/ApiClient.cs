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

    public async Task<(HttpResponseMessage, TOutput)> Post<TOutput>(
        string resourceUrl,
        object request
    ) where TOutput : class
    {
        var streamContent = Serialize(request);

        var response = await _httpClient.PostAsync(
            resourceUrl,
            streamContent
        );

        var output = await Deseriealize<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string resourceUrl,
        object request
    ) where TOutput : class
    {
        var streamContent = Serialize(request);

        var response = await _httpClient.PutAsync(
            resourceUrl,
            streamContent
        );

        var output = await Deseriealize<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(string resourceUrl) where TOutput : class
    {
        var response = await _httpClient.GetAsync(resourceUrl);

        var output = await Deseriealize<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(string resourceUrl) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(resourceUrl);

        var output = await Deseriealize<TOutput>(response);

        return (response, output);
    }

    private static StringContent Serialize(object request) => new(
        JsonSerializer.Serialize(
            request,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        ),
        Encoding.UTF8,
        "application/json"
    );

    private static async Task<TOutput> Deseriealize<TOutput>(HttpResponseMessage response) where TOutput : class
    {
        var stream = response.Content.ReadAsStream();

        TOutput? output = null;
        if (stream != null)
        {
            output = await JsonSerializer.DeserializeAsync<TOutput>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false
                }
            );
        }

        return output!;
    }
}
