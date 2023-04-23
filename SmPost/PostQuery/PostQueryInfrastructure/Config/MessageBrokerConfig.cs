namespace PostQueryInfrastructure.Config;

public class MessageBrokerConfig
{
    public string Hosts { get; set; }
    
    public string TopicName { get; set; }
    
    /// <summary>
    /// Will force message broker to consume topic again from the beginning
    /// </summary>
    public bool ConsumeFromBeginning { get; set; }
    
    public ConsumerConfig ConsumerConfig { get; set; }
}