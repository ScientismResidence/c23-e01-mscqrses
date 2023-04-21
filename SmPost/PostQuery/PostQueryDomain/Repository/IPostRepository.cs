using PostQueryDomain.Entity;

namespace PostQueryDomain.Repository;

public interface IPostRepository
{
    Task<PostEntity> GetByIdAsync(Guid postId);
    
    Task<List<PostEntity>> GetAllAsync();
    
    Task<List<PostEntity>> GetByAuthorAsync(string author);
    
    Task<List<PostEntity>> GetWithLikesAsync(int likes);
    
    Task<List<PostEntity>> GetWithCommentsAsync();

    Task CreateAsync(PostEntity post);
    
    Task UpdateAsync(PostEntity post);
    
    Task DeleteAsync(Guid postId);
}