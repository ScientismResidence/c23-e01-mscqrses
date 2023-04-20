using System.Reflection;
using CqrsCore.Event;
using CqrsCore.Message;

namespace CqrsCore.Domain;

public class AggregateRoot
{
    private readonly List<EventBase> _events = new();
    
    public Guid Id { get; protected set; }

    public int Version { get; set; } = -1;
    
    public IEnumerable<EventBase> GetUncommittedEvents()
    {
        return _events;
    }
    
    public void MarkEventsAsCommitted()
    {
        _events.Clear();
    }
    
    public void ReplayEvents(IEnumerable<EventBase> events)
    {
        foreach (EventBase @event in events)
        {
            ApplyChange(@event, false);
        }
    }
    
    protected void ApplyChange(EventBase @event, bool isNew)
    {
        MethodInfo method = this.GetType().GetMethod("Apply", new[] { @event.GetType() });

        if (method is null)
        {
            throw new ArgumentNullException(
                $"The apply method wasn't found in aggregate {this.GetType().Name} " +
                $"for event {@event.GetType().Name}");
        }
        
        method.Invoke(this, new object[] { @event });
        
        if (isNew)
        {
            _events.Add(@event);
        }
    }
    
    protected void RaiseEvent(EventBase @event)
    {
        ApplyChange(@event, true);
    }
}