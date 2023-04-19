using CqrsCore.Event;

namespace PostCommon.Event;

public class PostLikedEvent : EventBase
{
    public PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}