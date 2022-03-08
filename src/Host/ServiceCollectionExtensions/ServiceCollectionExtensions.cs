using Hellang.Middleware.ProblemDetails;
using Marten;
using Marten.Services;
using System.Text.Json.Serialization;
using TrailRunning.Races.Core.Serialization;
using TrailRunning.Races.Core.Threading;
using TrailRunning.Races.Management.Host.Configuration;
using TrailRunning.Races.Management.Host.Features.Races;
using Weasel.Core;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services) =>
        services
        .AddMvcCore()
        .Services
        .AddProblemDetails(opts =>
        {
            opts.IncludeExceptionDetails = (ctx, ex) =>
            {
                // Fetch services from HttpContext.RequestServices
                var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment() || env.IsStaging();
            };
            ConfigureProblemDetails(opts);
        });

    private static void ConfigureProblemDetails(ProblemDetailsOptions opts) => opts.MapRaces();

#pragma warning disable IDE1006 // Naming Styles
    private const string DefaultConfigKey = "EventStore";
#pragma warning restore IDE1006 // Naming Styles

    public static IServiceCollection AddCustomMarten(this IServiceCollection services,
        Action<StoreOptions>? configureOptions = null, string configKey = DefaultConfigKey)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var martenConfig = configuration.GetSection(configKey).Get<MartenConfig>();

        var documentStore = services
            .AddMarten(options => SetStoreOptions(options, martenConfig, configureOptions))
            .AddAsyncDaemon(Marten.Events.Daemon.Resiliency.DaemonMode.Solo)
            .InitializeStore();

        SetupSchema(documentStore, martenConfig, 1);

        return services;
    }

    private static void SetStoreOptions(StoreOptions options, MartenConfig config, Action<StoreOptions>? configureOptions = null)
    {
        options.Connection(config.ConnectionString);
        options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

        var schemaName = Environment.GetEnvironmentVariable("SchemaName");
        options.Events.DatabaseSchemaName = schemaName ?? config.WriteModelSchema;
        options.DatabaseSchemaName = schemaName ?? config.ReadModelSchema;

        var serializer = new SystemTextJsonSerializer
        {
            EnumStorage = EnumStorage.AsString,
            Casing = Casing.CamelCase,
        };
        serializer.Customize(jsonSerializerOptions =>
        {
            jsonSerializerOptions.Converters.Add(new DateOnlyConverter());
            jsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        options.Serializer(serializer);

        configureOptions?.Invoke(options);
    }

    private static void SetupSchema(IDocumentStore documentStore, MartenConfig martenConfig, int retryLeft = 1)
    {
        try
        {
            if (martenConfig.ShouldRecreateDatabase)
            {
                documentStore.Advanced.Clean.CompletelyRemoveAll();
            }

            using (NoSynchronizationContextScope.Enter())
            {
#pragma warning disable CS0618 // Type or member is obsolete
                documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
        catch
        {
            if (retryLeft == 0)
            {
                throw;
            }

            Thread.Sleep(1000);
            SetupSchema(documentStore, martenConfig, --retryLeft);
        }
    }
}
