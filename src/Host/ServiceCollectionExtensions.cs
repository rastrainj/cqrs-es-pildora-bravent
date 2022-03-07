using Hellang.Middleware.ProblemDetails;
using TrailRunning.Races.Management.Host.Features.Races;

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
}
