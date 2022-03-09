namespace TrailRunning.Races.Management.Host.Features.Races;

public static class RacesEndpoints
{
    public static void MapRaces(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/races", PlanRace.PlanRace.Handle);
        endpoints.MapGet("/api/races", GetAllRaces.GetAllRaces.Handle);
        endpoints.MapGet("/api/races/{raceId}", GetRaceById.GetRaceById.Handle);
    }
}
