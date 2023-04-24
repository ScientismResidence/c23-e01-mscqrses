using PostQueryApi.Query;
using PostQueryDomain.Entity;

namespace PostQueryApi.Handler;

public interface IQueryHandler
{
    Task<List<PostEntity>> HandleAsync(GetAllPostsQuery query);
    
    Task<List<PostEntity>> HandleAsync(GetPostByIdQuery query);
    
    Task<List<PostEntity>> HandleAsync(GetPostsByAuthorQuery query);
    
    Task<List<PostEntity>> HandleAsync(GetPostsWithCommentsQuery query);
    
    Task<List<PostEntity>> HandleAsync(GetPostsWithLikesQuery query);
}