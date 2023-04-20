using CqrsCore.Event;

namespace CqrsCore.Producer;

public interface IEventProducer
{
    Task ProduceAsync<TEvent>(TEvent @event) where TEvent : EventBase;
}