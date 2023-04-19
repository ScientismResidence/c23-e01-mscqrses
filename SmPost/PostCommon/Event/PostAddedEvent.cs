using CqrsCore.Event;

namespace PostCommon.Event;

public class PostAddedEvent : EventBase
{
    public PostAddedEvent() : base(nameof(PostAddedEvent))
    {
    }
    
    public string Author { get; set; }
    
    public string Message { get; set; }
    
    public DateTime CreatedAt { get; set; }
}