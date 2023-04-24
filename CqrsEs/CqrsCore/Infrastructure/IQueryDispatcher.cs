using CqrsCore.Query;

namespace CqrsCore.Infrastructure;

public interface IQueryDispatcher<TEntity>
{
    void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler)
        where TQuery : QueryBase;
    
    Task<List<TEntity>> SendAsync(QueryBase query);
}