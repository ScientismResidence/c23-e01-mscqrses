using System.Text.Json;
using Confluent.Kafka;
using CqrsCore.Consumer;
using CqrsCore.Event;
using Microsoft.Extensions.Options;
using PostQueryInfrastructure.Config;
using PostQueryInfrastructure.Converter;
using PostQueryInfrastructure.Handlers;
using ConsumerConfig = Confluent.Kafka.ConsumerConfig;

namespace PostQueryInfrastructure.Consumer;

public class PostEventConsumer : IEventConsumer
{
    private readonly MessageBrokerConfig _config;
    private readonly IPostEventHandler _handler;

    public PostEventConsumer(
        IOptions<MessageBrokerConfig> config, IPostEventHandler handler)
    {
        _config = config.Value;
        _handler = handler;
    }

    public void Consume()
    {
        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = _config.Hosts,
            GroupId = _config.ConsumerConfig.GroupId,
            AutoOffsetReset = (AutoOffsetReset)_config.ConsumerConfig.AutoOffsetReset,
            EnableAutoCommit = _config.ConsumerConfig.EnableAutoCommit,
            AllowAutoCreateTopics = _config.ConsumerConfig.AllowAutoCreateTopics
        };
        
        IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();
        
        consumer.Subscribe(_config.TopicName);

        while (true)
        {
            ConsumeResult<string, string> consumeResult = consumer.Consume();
            
            if (consumeResult.IsPartitionEOF)
            {
                continue;
            }

            JsonSerializerOptions options = new()
            {
                Converters =
                {
                    new JsonEventConverter()
                }
            };
            
            EventBase @event = JsonSerializer.Deserialize<EventBase>(consumeResult.Message.Value, options);
            var handlerMethod = _handler.GetType()
                .GetMethod(nameof(IPostEventHandler.On), new Type[] { @event.GetType() });
            ArgumentNullException.ThrowIfNull(handlerMethod);
            handlerMethod.Invoke(_handler, new object[] { @event });

            consumer.Commit(consumeResult);
        }
    }
}