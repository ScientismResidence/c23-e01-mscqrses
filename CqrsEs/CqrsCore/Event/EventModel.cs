using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CqrsCore.Event;

public class EventModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public int Version { get; set; }
    
    public Guid AggregateId { get; set; }
    
    public string AggregateType { get; set; }
    
    public string EventType { get; set; }
    
    public EventBase Event { get; set; }
}