namespace PostQueryInfrastructure.Config;

public class ConsumerConfig
{
    public string GroupId { get; set; }
    
    public int AutoOffsetReset { get; set; }
    
    public bool EnableAutoCommit { get; set; }
    
    public bool AllowAutoCreateTopics { get; set; }
}