using System.Text.Json;
using Confluent.Kafka;
using CqrsCore.Event;
using CqrsCore.Producer;
using Microsoft.Extensions.Options;
using PostCmdInfrastructure.Config;

namespace PostCmdInfrastructure;

public class EventProducer : IEventProducer
{
    private readonly MessageBrokerConfig _config;

    public EventProducer(IOptions<MessageBrokerConfig> config)
    {
        _config = config.Value;
    }
    
    public async Task ProduceAsync<TEvent>(TEvent @event) where TEvent : EventBase
    {
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = _config.Hosts
        };
        using IProducer<string, string> producer = new ProducerBuilder<string, string>(config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        Message<string, string> message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };

        DeliveryResult<string, string> result = await producer.ProduceAsync(_config.TopicName, message);

        if (result.Status == PersistenceStatus.NotPersisted)
        {
            throw new ApplicationException(
                $"Unable to produce message [{nameof(@event)}] to topic [{_config.TopicName}] " +
                $"due the following reason: {result.Message}");
        }
    }
}