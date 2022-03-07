using System.Text.Json;
using TrailRunning.Races.Core.Serialization;

namespace TrailRunning.Races.Management.FunctionalTests.Extensions;

internal static class JsonSerializerOptionsConstants
{
    internal static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Converters =
        {
            new DateOnlyConverter(),
            new TimeOnlyConverter()
        }
    };
}
