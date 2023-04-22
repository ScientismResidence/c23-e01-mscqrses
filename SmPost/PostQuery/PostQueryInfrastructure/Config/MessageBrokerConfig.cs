namespace PostQueryInfrastructure.Config;

public class MessageBrokerConfig
{
    public string Hosts { get; set; }
    
    public string TopicName { get; set; }
    
    public ConsumerConfig ConsumerConfig { get; set; }
}