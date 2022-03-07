using Microsoft.AspNetCore.Mvc;
using TrailRunning.Races.Management.Domain.Races;

namespace TrailRunning.Races.Management.Host.Features.Races.PlanRace;

public static class PlanRace
{
    [ApiExplorerSettings(GroupName = "Races")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status409Conflict)]
    public static async Task<IResult> Handle(PlanRaceRequest request, IMediator mediator)
    {
        var raceId = Guid.NewGuid();
        await mediator.Send(PlanRaceCommand.FromRequest(raceId, request));

        return Results.Created($"/api/races/{raceId}", raceId);
    }
}

public record PlanRaceRequest(DateOnly Date, TimeOnly? Hour, string Town, double? Distance, double? ElevationGain);

public record PlanRaceCommand(Guid RaceId, RaceDate Date, RaceLocation Location, RaceTechnicalData? TechnicalData) : IRequest
{
    internal static PlanRaceCommand FromRequest(Guid RaceId, PlanRaceRequest request)
    {
        var date = RaceDate.Create(request.Date, request.Hour);
        var location = RaceLocation.Create(request.Town);
        var technicalData = request.Distance is not null ? RaceTechnicalData.Create(request.Distance.Value, request.ElevationGain ?? 0) : null;

        return new(RaceId, date, location, technicalData);
    }
}

public class PlanRaceCommandHandler : IRequestHandler<PlanRaceCommand>
{
    public async Task<Unit> Handle(PlanRaceCommand request, CancellationToken cancellationToken)
    {
        var race = Race.Plan(request.RaceId, request.Date, request.Location, request.TechnicalData);

        return Unit.Value;
    }
}
