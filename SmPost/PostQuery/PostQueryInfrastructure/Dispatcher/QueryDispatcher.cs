using CqrsCore.Infrastructure;
using CqrsCore.Query;
using PostQueryDomain.Entity;

namespace PostQueryInfrastructure.Dispatcher;

public class QueryDispatcher : IQueryDispatcher<PostEntity>
{
    private readonly Dictionary<Type, Func<QueryBase, Task<List<PostEntity>>>> _handlers = new();

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler)
        where TQuery : QueryBase
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
        }

        _handlers.Add(typeof(TQuery), query => handler((TQuery)query));
    }

    public Task<List<PostEntity>> SendAsync(QueryBase query)
    {
        if (!_handlers.TryGetValue(query.GetType(), out Func<QueryBase, Task<List<PostEntity>>> handler))
        {
            throw new ArgumentNullException(nameof(handler), 
                $"No query handler was registered for query of [{query.GetType().Name}] type!");
        }
        
        return handler(query);
    }
}