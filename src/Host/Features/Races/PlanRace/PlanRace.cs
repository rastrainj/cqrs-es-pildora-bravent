using Microsoft.AspNetCore.Mvc;
using TrailRunning.Races.Core.Repository;
using TrailRunning.Races.Management.Domain.Races;

namespace TrailRunning.Races.Management.Host.Features.Races.PlanRace;

public static class PlanRace
{
    [ApiExplorerSettings(GroupName = "Races")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public static async Task<IResult> Handle(PlanRaceRequest request, IMediator mediator)
    {
        var raceId = Guid.NewGuid();
        await mediator.Send(PlanRaceCommand.FromRequest(raceId, request));

        return Results.Created($"/api/races/{raceId}", raceId);
    }
}

public record PlanRaceRequest(string Name, DateOnly Date, TimeOnly? Hour, string Town, double? Distance, double? ElevationGain);

public record PlanRaceCommand(Guid RaceId, RaceName Name, RaceDate Date, RaceLocation Location, RaceTechnicalData? TechnicalData) : IRequest
{
    internal static PlanRaceCommand FromRequest(Guid RaceId, PlanRaceRequest request)
    {
        var name = RaceName.Create(request.Name);
        var date = RaceDate.Create(request.Date, request.Hour);
        var location = RaceLocation.Create(request.Town);
        var technicalData = request.Distance is not null ? RaceTechnicalData.Create(request.Distance.Value, request.ElevationGain ?? 0) : null;

        return new(RaceId, name, date, location, technicalData);
    }
}

public class PlanRaceCommandHandler : IRequestHandler<PlanRaceCommand>
{
    private readonly IMartenRepository<Race> _martenRepository;

    public PlanRaceCommandHandler(IMartenRepository<Race> martenRepository)
        => _martenRepository = martenRepository ?? throw new ArgumentNullException(nameof(martenRepository));

    public async Task<Unit> Handle(PlanRaceCommand request, CancellationToken cancellationToken)
    {
        var race = Race.Plan(request.RaceId, request.Name, request.Date, request.Location, request.TechnicalData);

        await _martenRepository.AddAsync(race, cancellationToken);

        return Unit.Value;
    }
}
