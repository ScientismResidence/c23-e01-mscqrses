using CqrsCore.Domain;
using CqrsCore.Event;
using CqrsCore.Exception;
using CqrsCore.Infrastructure;
using PostCmdDomain;

namespace PostCmdInfrastructure;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _repository;
    
    public EventStore(IEventStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EventBase>> GetEventsForAggregateAsync(Guid aggregateId)
    {
        List<EventModel> events = await _repository.FindByAggregateIdAsync(aggregateId);

        if (events is null || !events.Any())
        {
            throw new AggregateNotFoundException("Aggregate not found");
        }
        
        return events
            .OrderBy(value => value.Version)
            .Select(value => value.Event)
            .ToList();
    }
    
    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<EventBase> events, int expectedVersion)
    {
        List<EventModel> storeEvents = await _repository.FindByAggregateIdAsync(aggregateId);
        
        if (expectedVersion != -1 || storeEvents[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException("Concurrency exception");
        }

        int version = expectedVersion;
        
        foreach (EventBase @event in events)
        {
            await _repository.SaveAsync(new EventModel
            {
                AggregateId = aggregateId,
                AggregateType = nameof(PostAggregate),
                EventType = @event.GetType().Name,
                TimeStamp = DateTime.UtcNow,
                Version = ++version,
                Event = @event
            });
        }
    }
}