using Marten;
using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Host.Features.Races.GetAllRaces;
using TrailRunning.Races.Management.Host.Features.Races.GetRaceById;

namespace TrailRunning.Races.Management.Host.Features.Races;

public static class RacesProjections
{
    internal static void ConfigureRaces(this StoreOptions options)
    {
        // Snapshots
        options.Projections.SelfAggregate<Race>();

        // Projections
        options.Projections.Add<RaceShortInfoProjection>();
        options.Projections.Add<RaceDetailsProjection>();
    }
}
