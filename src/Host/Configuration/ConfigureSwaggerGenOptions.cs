using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TrailRunning.Races.Management.Host.Configuration;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();
        options.CustomSchemaIds(x => x.FullName);

        options.MapType<DateOnly>(() => new()
        {
            Type = "string",
            Example = new OpenApiString("31-12-1999"),
        });
        options.MapType<TimeOnly>(() => new()
        {
            Type = "string",
            Example = new OpenApiString("21:59"),
        });

    }
}
