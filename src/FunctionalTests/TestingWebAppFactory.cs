using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrailRunning.Races.Core.Aggregates;
using TrailRunning.Races.Core.Repository;

namespace TrailRunning.Races.Management.FunctionalTests;

public class TestingWebAppFactory : WebApplicationFactory<Program>
{
    private static ServiceProvider _serviceProvider = default!;

    public Given Given { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("tests");

        builder.ConfigureServices(services =>
        {
            _serviceProvider = services.BuildServiceProvider();
        });

        Given = new Given(this);
    }

    public Task<T?> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T?>> func) where T : class => func(_serviceProvider);

    public Task<T?> ExecuteAsync<T>(Func<IMartenRepository<T>, Task<T?>> func) where T : class, IAggregate
        => ExecuteScopeAsync(sp => func(_serviceProvider.GetService<IMartenRepository<T>>()!));
}
