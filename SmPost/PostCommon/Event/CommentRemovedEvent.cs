using CqrsCore.Event;

namespace PostCommon.Event;

public class CommentRemovedEvent : EventBase
{
    public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
    {
    }
    
    public Guid CommentId { get; set; }
    
    public string Author { get; set; }
}