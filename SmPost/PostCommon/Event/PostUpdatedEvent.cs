using CqrsCore.Event;

namespace PostCommon.Event;

public class PostUpdatedEvent : EventBase
{
    public PostUpdatedEvent() : base(nameof(PostUpdatedEvent))
    {
    }
    
    public string Message { get; set; }
}