using TrailRunning.Races.Core.Aggregates;
using TrailRunning.Races.Management.Domain.Races.Events;

namespace TrailRunning.Races.Management.Domain.Races;

public class Race : Aggregate
{
    public RaceName Name { get; private set; } = default!;
    public RaceDate Date { get; private set; } = default!;
    public RaceLocation Location { get; private set; } = default!;
    public RaceTechnicalData? TechnicalData { get; private set; }
    public RaceStatus Status { get; private set; }

    public static Race Plan(Guid raceId, RaceName name, RaceDate date, RaceLocation location, RaceTechnicalData? technicalData)
        => new(raceId, name, date, location, technicalData);

    private Race() { }

    private Race(Guid raceId, RaceName name, RaceDate date, RaceLocation location, RaceTechnicalData? technicalData)
    {
        var @event = new RacePlannedEvent(raceId, name.Name, date.Date, date.Hour, location.Town, technicalData?.Distance, technicalData?.ElevationGain);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RacePlannedEvent @event)
    {
        Version++;

        Id = @event.RaceId;
        Name = RaceName.Create(@event.Name);
        Date = RaceDate.Create(@event.Date, @event.Hour);
        Location = RaceLocation.Create(@event.Town);
        TechnicalData = @event.Distance is not null ? RaceTechnicalData.Create(@event.Distance.Value, @event.ElevationGain!.Value) : null;

        Status = RaceStatus.Planned;

    }
}
