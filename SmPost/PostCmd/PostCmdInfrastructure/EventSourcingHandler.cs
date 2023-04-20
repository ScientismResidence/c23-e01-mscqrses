using CqrsCore.Event;
using CqrsCore.Handler;
using CqrsCore.Infrastructure;
using PostCmdDomain;

namespace PostCmdInfrastructure;

public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _store;
    
    public EventSourcingHandler(IEventStore store)
    {
        _store = store;
    }
    
    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        List<EventBase> events = await _store.GetEventsForAggregateAsync(aggregateId);
        PostAggregate aggregate = new();
        
        if (events is null || !events.Any())
        {
            return aggregate;
        }
        
        aggregate.ReplayEvents(events);
        aggregate.Version = events[^1].Version;

        return aggregate;
    }

    public async Task SaveAsync(PostAggregate aggregate)
    {
        await _store.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedEvents(), aggregate.Version);
        aggregate.MarkEventsAsCommitted();
    }
}