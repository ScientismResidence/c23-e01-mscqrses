using CqrsCore.Event;

namespace PostCommon.Event;

public class CommentUpdatedEvent : EventBase
{
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
    {
    }
    
    public Guid CommentId { get; set; }
    
    public string Message { get; set; }
    
    public string Author { get; set; }
}