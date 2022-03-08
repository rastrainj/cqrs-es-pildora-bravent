using Marten;
using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Host.Features.Races.GetAllRaces;

namespace TrailRunning.Races.Management.Host.Features.Races;

public static class RacesProjections
{
    internal static void ConfigureRaces(this StoreOptions options)
    {
        // Snapshots
        options.Projections.SelfAggregate<Race>();

        // Projections
        options.Projections.Add<RaceShortInfoProjection>();
    }
}
