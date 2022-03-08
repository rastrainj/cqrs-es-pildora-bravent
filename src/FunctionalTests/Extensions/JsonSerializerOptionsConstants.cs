using System.Dynamic;
using System.Text.Json.Serialization;
using TrailRunning.Races.Core.Response;
using TrailRunning.Races.Core.Serialization;

namespace TrailRunning.Races.Management.FunctionalTests.Extensions;

internal static class JsonSerializerOptionsConstants
{
    internal static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        IncludeFields = true,
        Converters =
        {
            new DateOnlyConverter(),
            new TimeOnlyConverter(),
            new JsonStringEnumConverter()
        }
    };
}
