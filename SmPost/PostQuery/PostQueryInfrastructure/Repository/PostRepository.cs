using Microsoft.EntityFrameworkCore;
using PostQueryDomain.Entity;
using PostQueryDomain.Repository;

namespace PostQueryInfrastructure.Repository;

public class PostRepository : IPostRepository
{
    private readonly SocialMediaDbContext _context;
    
    public PostRepository(SocialMediaDbContext context)
    {
        _context = context;
    }
    
    public async Task<PostEntity> GetByIdAsync(Guid postId)
    {
        PostEntity post = await _context.Posts
            .Include(post => post.Comments)
            .FirstOrDefaultAsync(post => post.PostId == postId);

        return post;
    }

    public async Task<List<PostEntity>> GetAllAsync()
    {
        List<PostEntity> posts = await _context.Posts
            .AsNoTracking()
            .Include(post => post.Comments)
            .ToListAsync();

        return posts;
    }

    public async Task<List<PostEntity>> GetByAuthorAsync(string author)
    {
        List<PostEntity> posts = await _context.Posts
            .AsNoTracking()
            .Include(post => post.Comments)
            .Where(post => post.Author == author)
            .ToListAsync();

        return posts;
    }

    public async Task<List<PostEntity>> GetWithLikesAsync(int likes)
    {
        List<PostEntity> posts = await _context.Posts
            .AsNoTracking()
            .Include(post => post.Comments)
            .Where(post => post.Likes >= likes)
            .ToListAsync();

        return posts;
    }

    public async Task<List<PostEntity>> GetWithCommentsAsync()
    {
        List<PostEntity> posts = await _context.Posts
            .AsNoTracking()
            .Include(post => post.Comments)
            .Where(post => post.Comments.Any())
            .ToListAsync();

        return posts;
    }

    public async Task CreateAsync(PostEntity post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        PostEntity post = await GetByIdAsync(postId);

        if (post == null)
        {
            return;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }
}