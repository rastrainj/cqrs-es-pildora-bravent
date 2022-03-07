using TrailRunning.Races.Core.Aggregates;
using TrailRunning.Races.Core.Events;

namespace TrailRunning.Races.Management.DomainTests;

public static class AggregateExtensions
{
    public static T? PublishedEvent<T>(this IAggregate aggregate) where T : class, IEvent
        => aggregate.DequeueUncommittedEvents().LastOrDefault() as T;
}
