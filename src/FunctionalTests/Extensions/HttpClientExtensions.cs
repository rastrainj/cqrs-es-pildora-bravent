using TrailRunning.Races.Management.FunctionalTests.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Net.Http;
#pragma warning restore IDE0130 // Namespace does not match folder structure


public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PostAsJsonAsync<TRequest>(this HttpClient client, string requestUri, TRequest request)
        => client.PostAsJsonAsync(requestUri, request, JsonSerializerOptionsConstants._serializerOptions);
    public static Task<TValue?> GetFromJsonAsync<TValue>(this HttpClient client, string? requestUri)
        => client.GetFromJsonAsync<TValue>(requestUri, JsonSerializerOptionsConstants._serializerOptions);
}
