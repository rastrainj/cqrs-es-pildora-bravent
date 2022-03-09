namespace TrailRunning.Races.Management.Domain.Races.Events;

public record RacePlannedEvent(Guid RaceId, string Name, DateOnly Date, TimeOnly? Hour, string Town, double? Distance, double? ElevationGain) : IEvent;
