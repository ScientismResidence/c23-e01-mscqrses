using PostQueryApi.Query;
using PostQueryDomain.Entity;
using PostQueryDomain.Repository;

namespace PostQueryApi.Handler;

public class QueryHandler : IQueryHandler
{
    private readonly IPostRepository _repository;
    
    public QueryHandler(IPostRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<PostEntity>> HandleAsync(GetAllPostsQuery query)
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(GetPostByIdQuery query)
    {
        return new List<PostEntity>
        {
            await _repository.GetByIdAsync(query.Id)
        };
    }

    public async Task<List<PostEntity>> HandleAsync(GetPostsByAuthorQuery query)
    {
        return await _repository.GetByAuthorAsync(query.Author);
    }

    public async Task<List<PostEntity>> HandleAsync(GetPostsWithCommentsQuery query)
    {
        return await _repository.GetWithCommentsAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(GetPostsWithLikesQuery query)
    {
        return await _repository.GetWithLikesAsync(query.Likes);
    }
}