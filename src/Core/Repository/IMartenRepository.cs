using Marten;
using TrailRunning.Races.Core.Aggregates;

namespace TrailRunning.Races.Core.Repository;
public interface IMartenRepository<T> where T : class, IAggregate
{
    Task<T?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<long> AddAsync(T aggregate, CancellationToken cancellationToken);
}

public class MartenRepository<T> : IMartenRepository<T> where T : class, IAggregate
{
    private readonly IDocumentSession _documentSession;

    public MartenRepository(IDocumentSession documentSession) => _documentSession = documentSession;

    public Task<T?> FindAsync(Guid id, CancellationToken cancellationToken)
        => _documentSession.Events.AggregateStreamAsync<T>(id, token: cancellationToken);

    public async Task<long> AddAsync(T aggregate, CancellationToken cancellationToken)
    {
        var events = aggregate.DequeueUncommittedEvents();

        _documentSession.Events.StartStream<Aggregate>(
            aggregate.Id,
            events
        );

        await _documentSession.SaveChangesAsync(cancellationToken);

        return events.Length;
    }
}
