namespace TrailRunning.Races.Management.Domain.Races.Events;

public record RacePlannedEvent(Guid RaceId, DateOnly Date, TimeOnly? Hour, string Location, double? Distance, double? ElevationGain) : IEvent;
