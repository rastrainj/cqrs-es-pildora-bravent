using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using TrailRunning.Races.Core.Aggregates;
using TrailRunning.Races.Core.Repository;
using TrailRunning.Races.Management.Host.Configuration;

namespace TrailRunning.Races.Management.FunctionalTests;

public class TestingWebAppFactory : WebApplicationFactory<Program>
{
    private static ServiceProvider _serviceProvider = default!;
    private static readonly Checkpoint _checkpoint = new();

    public Given Given { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("tests");

        builder.ConfigureServices(services => _serviceProvider = services.BuildServiceProvider());

        _checkpoint.DbAdapter = DbAdapter.Postgres;
        Given = new Given(this);
    }

    internal static void ResetDatabase()
    {
        using var scope = _serviceProvider.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var martenConfig = configuration.GetSection("EventStore").Get<MartenConfig>();
        var options = scope.ServiceProvider.GetService<StoreOptions>();

        _checkpoint.WithReseed = false;
        _checkpoint.SchemasToInclude = new string[] { options!.Events.DatabaseSchemaName, options.DatabaseSchemaName, "public" };

        using var connection = new NpgsqlConnection(martenConfig.ConnectionString);
        connection.Open();

        _checkpoint.Reset(connection).Wait();
    }

    public async Task<T?> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T?>> func) where T : class
    {
        using var scope = _serviceProvider.CreateScope();
        return await func(scope.ServiceProvider);
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = _serviceProvider.CreateScope();
        await func(scope.ServiceProvider);
    }

    public Task<T?> ExecuteAsync<T>(Func<IMartenRepository<T>, Task<T?>> func) where T : class, IAggregate
        => ExecuteScopeAsync(sp => func(sp.GetService<IMartenRepository<T>>()!));

    public async Task ExecuteAsync<T>(Func<IMartenRepository<T>, Task> func) where T : class, IAggregate
        => await ExecuteScopeAsync(sp => func(sp.GetService<IMartenRepository<T>>()!));
}
