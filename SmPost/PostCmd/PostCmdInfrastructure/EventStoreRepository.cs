using CqrsCore.Domain;
using CqrsCore.Event;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PostCmdInfrastructure.Config;

namespace PostCmdInfrastructure;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStore;

    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        MongoClient client = new MongoClient(config.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(config.Value.DatabaseName);
        _eventStore = database.GetCollection<EventModel>(config.Value.CollectionName);
    }

    public async Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId)
    {
        return await _eventStore
            .Find(x => x.AggregateId == aggregateId)
            .ToListAsync()
            .ConfigureAwait(false);
    }
    
    public async Task SaveAsync(EventModel @event)
    {
        await _eventStore
            .InsertOneAsync(@event)
            .ConfigureAwait(false);
    }
}