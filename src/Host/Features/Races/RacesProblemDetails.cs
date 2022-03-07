using Hellang.Middleware.ProblemDetails;
using TrailRunning.Races.Management.Domain.Races.Exceptions;

namespace TrailRunning.Races.Management.Host.Features.Races;

public static class RacesProblemDetails
{
    public static void MapRaces(this ProblemDetailsOptions options)
    {
        options.Map<RaceDatePastException>(exception =>
            new()
            {
                Title = "Business rule broken",
                Status = StatusCodes.Status409Conflict,
                Detail = exception.Message,
                Type = $"https://httpstatuses.com/409",
            });
    }
}
