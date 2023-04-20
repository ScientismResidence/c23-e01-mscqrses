using System.Reflection;
using CqrsCore.Message;

namespace CqrsCore;

public class AggregateRoot
{
    private readonly List<MessageBase> _events = new();
    
    public Guid Id { get; protected set; }

    public int Version { get; set; } = -1;
    
    public IEnumerable<MessageBase> GetUncommittedEvents()
    {
        return _events;
    }
    
    public void MarkEventsAsCommitted()
    {
        _events.Clear();
    }
    
    public void ReplayEvents(IEnumerable<MessageBase> events)
    {
        foreach (MessageBase @event in events)
        {
            ApplyChange(@event, false);
        }
    }
    
    protected void ApplyChange(MessageBase @event, bool isNew)
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
    
    protected void RaiseEvent(MessageBase @event)
    {
        ApplyChange(@event, true);
    }
}