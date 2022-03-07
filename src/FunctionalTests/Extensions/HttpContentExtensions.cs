using TrailRunning.Races.Management.FunctionalTests.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Net.Http;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class HttpContentExtensions
{
    public static async Task<TEntity?> ReadAsAsync<TEntity>(this HttpContent httpContent)
    {
        var body = await httpContent.ReadAsStringAsync();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));

        return await JsonSerializer.DeserializeAsync<TEntity>(stream, options: JsonSerializerOptionsConstants._serializerOptions);
    }
}
