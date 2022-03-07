using TrailRunning.Races.Core.Aggregates;
using TrailRunning.Races.Management.Domain.Races.Events;

namespace TrailRunning.Races.Management.Domain.Races;

public class Race : Aggregate
{
    public RaceDate Date { get; private set; } = default!;
    public RaceLocation Location { get; private set; } = default!;
    public RaceTechnicalData? TechnicalData { get; private set; }
    public RaceStatus Status { get; private set; }

    public static Race Plan(Guid raceId, RaceDate date, RaceLocation location, RaceTechnicalData? technicalData)
        => new(raceId, date, location, technicalData);

    private Race(Guid raceId, RaceDate date, RaceLocation location, RaceTechnicalData? technicalData)
    {
        var @event = new RacePlannedEvent(raceId, date.Date, date.Hour, location.Town, technicalData?.Distance, technicalData?.ElevationGain);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RacePlannedEvent @event)
    {
        Version++;

        Id = @event.RaceId;
        Date = RaceDate.Create(@event.Date, @event.Hour);
        Location = RaceLocation.Create(@event.Location);
        TechnicalData = @event.Distance is not null ? RaceTechnicalData.Create(@event.Distance.Value, @event.ElevationGain.Value) : null;

        Status = RaceStatus.Planned;

    }
}