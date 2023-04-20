using CqrsCore.Event;

namespace CqrsCore.Domain;

public interface IEventStoreRepository
{
    Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId);
    
    Task SaveAsync(EventModel @event);
}