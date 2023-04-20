using CqrsCore.Event;

namespace CqrsCore.Infrastructure;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<EventBase> events, int expectedVersion);
    
    Task<List<EventBase>> GetEventsForAggregateAsync(Guid aggregateId);
}