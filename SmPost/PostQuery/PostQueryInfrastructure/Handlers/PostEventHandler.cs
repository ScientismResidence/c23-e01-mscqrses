using PostCommon.Event;
using PostQueryDomain.Entity;
using PostQueryDomain.Repository;

namespace PostQueryInfrastructure.Handlers;

public class PostEventHandler : IPostEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    
    public PostEventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    
    public async Task On(PostAddedEvent @event)
    {
        PostEntity post = new PostEntity
        {
            PostId = @event.Id,
            Message = @event.Message,
            Author = @event.Author,
            CreatedAt = DateTime.UtcNow
        };

        await _postRepository.CreateAsync(post);
    }

    public async Task On(PostUpdatedEvent @event)
    {
        PostEntity post = await _postRepository.GetByIdAsync(@event.Id);
        ArgumentNullException.ThrowIfNull(post);
        
        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        PostEntity post = await _postRepository.GetByIdAsync(@event.Id);
        ArgumentNullException.ThrowIfNull(post);
        
        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
    }

    public async Task On(CommentAddedEvent @event)
    {
        CommentEntity comment = new CommentEntity
        {
            CommentId = @event.CommentId,
            PostId = @event.Id,
            Message = @event.Message,
            Author = @event.Author,
            CreatedAt = DateTime.UtcNow
        };
        
        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        CommentEntity comment = await _commentRepository.GetByIdAsync(@event.CommentId);
        ArgumentNullException.ThrowIfNull(comment);
        
        comment.Message = @event.Message;
        comment.IsEdited = true;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }
}