using CqrsCore.Event;

namespace PostCommon.Event;

public class PostRemovedEvent : EventBase
{
    public PostRemovedEvent() : base(nameof(PostRemovedEvent))
    {
    }
    
    public string Author { get; set; }
}