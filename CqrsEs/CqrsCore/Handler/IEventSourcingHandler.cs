using CqrsCore.Domain;

namespace CqrsCore.Handler;

public interface IEventSourcingHandler<TAggregate> where TAggregate : AggregateRoot, new()
{
    Task<TAggregate> GetByIdAsync(Guid aggregateId);
    
    Task SaveAsync(TAggregate aggregate);
}