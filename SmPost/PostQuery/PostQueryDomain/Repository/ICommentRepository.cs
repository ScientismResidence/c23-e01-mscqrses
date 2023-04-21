using PostQueryDomain.Entity;

namespace PostQueryDomain.Repository;

public interface ICommentRepository
{
    Task<CommentEntity> GetByIdAsync(Guid commentId);

    Task CreateAsync(CommentEntity comment);

    Task UpdateAsync(CommentEntity comment);
    
    Task DeleteAsync(Guid commentId);
}