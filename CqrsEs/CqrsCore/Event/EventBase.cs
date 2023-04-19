using CqrsCore.Message;

namespace CqrsCore.Event;

public abstract class EventBase : MessageBase
{
    protected EventBase(string type)
    {
        Type = type;
    }
    
    public int Version { get; set; }
    
    public string Type { get; }
}