using PostCommon.Event;

namespace PostQueryInfrastructure.Handlers;

public interface IPostEventHandler
{
    Task On(PostAddedEvent @event);
    
    Task On(PostUpdatedEvent @event);
    
    Task On(PostLikedEvent @event);
    
    Task On(PostRemovedEvent @event);
    
    Task On(CommentAddedEvent @event);
    
    Task On(CommentUpdatedEvent @event);
    
    Task On(CommentRemovedEvent @event);
}