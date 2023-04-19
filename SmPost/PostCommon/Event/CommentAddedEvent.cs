using CqrsCore.Event;

namespace PostCommon.Event;

public class CommentAddedEvent : EventBase
{
    public CommentAddedEvent() : base(nameof(CommentAddedEvent))
    {
    }
    
    public Guid CommentId { get; set; }
    
    public string Message { get; set; }
    
    public string Author { get; set; }
    
    public DateTime CreatedAt { get; set; }
}