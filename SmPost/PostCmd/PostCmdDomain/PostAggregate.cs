using CqrsCore;
using CqrsCore.Domain;
using PostCommon.Event;

namespace PostCmdDomain;

public class PostAggregate : AggregateRoot
{
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public PostAggregate()
    {
    }

    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostAddedEvent
        {
            Id = id,
            Author = author,
            Message = message,
            CreatedAt = DateTime.UtcNow
        });
    }
    
    public string Author { get; protected set; }
    
    public bool IsActive { get; protected set; }
    
    public void Apply(PostAddedEvent @event)
    {
        Id = @event.Id;
        Author = @event.Author;
        IsActive = true;
    }

    public void UpdatePost(string message)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot edit an inactive post");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("Invalid message parameter. Provide a valid message.");
        }
        
        RaiseEvent(new PostUpdatedEvent
        {
            Id = Id,
            Message = message
        });
    }
    
    public void Apply(PostUpdatedEvent @event)
    {
        Id = @event.Id;
    }

    public void LikePost()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot like an inactive post");
        }
        
        RaiseEvent(new PostLikedEvent
        {
            Id = Id
        });
    }
    
    public void Apply(PostLikedEvent @event)
    {
        Id = @event.Id;
    }
    
    public void AddComment(string author, string message)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot comment on an inactive post");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("Invalid message parameter. Provide a valid message.");
        }

        RaiseEvent(new CommentAddedEvent
        {
            Id = Id,
            CommentId = Guid.NewGuid(),
            Author = author,
            Message = message,
            CreatedAt = DateTime.UtcNow
        });
    }
    
    public void Apply(CommentAddedEvent @event)
    {
        Id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Author, @event.Message));
    }

    public void EditComment(Guid commentId, string message, string author)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot edit a comment on an inactive post");
        }
        
        if (!_comments[commentId].Item1.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot edit a comment that you did not create");
        }
        
        RaiseEvent(new CommentUpdatedEvent
        {
            Id = Id,
            CommentId = commentId,
            Message = message,
            Author = author,
            UpdatedAt = DateTime.UtcNow
        });
    }
    
    public void Apply(CommentUpdatedEvent @event)
    {
        Id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Author, @event.Message);
    }

    public void RemoveComment(Guid commentId, string author)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot remove a comment on an inactive post");
        }
        
        if (!_comments[commentId].Item1.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot remove a comment that you did not create");
        }
        
        RaiseEvent(new CommentRemovedEvent
        {
            Id = Id,
            CommentId = commentId
        });
    }
    
    public void Apply(CommentRemovedEvent @event)
    {
        Id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void RemovePost(string author)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("You cannot remove an inactive post");
        }
        
        if (!Author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot remove a post that you did not create");
        }
        
        RaiseEvent(new PostRemovedEvent
        {
            Id = Id
        });
    }
    
    public void Apply(PostRemovedEvent @event)
    {
        Id = @event.Id;
        IsActive = false;
    }
}