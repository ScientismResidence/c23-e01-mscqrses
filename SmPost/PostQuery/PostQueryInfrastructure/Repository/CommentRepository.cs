using PostQueryDomain.Entity;
using PostQueryDomain.Repository;

namespace PostQueryInfrastructure.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly SocialMediaDbContext _context;
    
    public CommentRepository(SocialMediaDbContext context)
    {
        _context = context;
    }
    
    public async Task<CommentEntity> GetByIdAsync(Guid commentId)
    {
        CommentEntity comment = await _context.Comments
            .FindAsync(commentId);

        return comment;
    }

    public async Task CreateAsync(CommentEntity comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        CommentEntity comment = await GetByIdAsync(commentId);

        if (comment is null)
        {
            return;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}