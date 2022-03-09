using Marten.Events.Aggregation;
using TrailRunning.Races.Management.Domain.Races;
using TrailRunning.Races.Management.Domain.Races.Events;

namespace TrailRunning.Races.Management.Host.Features.Races.GetAllRaces;

public class RaceShortInfo
{
    public Guid Id { get; set; }
    public RaceStatus Status { get; set; }
    public string Name { get; set; } = default!;

    public void Apply(RacePlannedEvent @event)
    {
        Id = @event.RaceId;
        Status = RaceStatus.Planned;
        Name = @event.Name;
    }
}

public class RaceShortInfoProjection : AggregateProjection<RaceShortInfo>
{
    public RaceShortInfoProjection()
    {
        ProjectEvent<RacePlannedEvent>((item, @event) => item.Apply(@event));
    }
}
