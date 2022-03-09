using Marten.Events.Aggregation;
using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Domain.Races.Events;

namespace TrailRunning.Races.Management.Host.Features.Races.GetRaceById;

public class RaceDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly Date { get; set; }
    public TimeOnly? Hour { get; set; }
    public string Town { get; set; } = default!;
    public double? Distance { get; set; }
    public double? ElevationGain { get; set; }
    public RaceStatus Status { get; set; }

    public void Apply(RacePlannedEvent @event)
    {
        Id = @event.RaceId;
        Status = RaceStatus.Planned;
        Name = @event.Name;
        Date = @event.Date;
        Hour = @event.Hour;
        Town = @event.Town;
        Distance = @event.Distance;
        ElevationGain = @event.ElevationGain;
    }
}

public class RaceDetailsProjection : AggregateProjection<RaceDetails>
{
    public RaceDetailsProjection()
    {
        ProjectEvent<RacePlannedEvent>((item, @event) => item.Apply(@event));
    }
}
