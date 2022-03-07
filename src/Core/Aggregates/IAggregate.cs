using TrailRunning.Races.Core.Events;

namespace TrailRunning.Races.Core.Aggregates;

public interface IAggregate : IAggregate<Guid>
{
}

public interface IAggregate<out T>
{
    T Id { get; }
    int Version { get; }

    IEvent[] DequeueUncommittedEvents();
}
