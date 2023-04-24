using System.Text.Json;
using Confluent.Kafka;
using CqrsCore.Consumer;
using CqrsCore.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostQueryInfrastructure.Config;
using PostQueryInfrastructure.Converter;
using PostQueryInfrastructure.Handlers;
using ConsumerConfig = Confluent.Kafka.ConsumerConfig;

namespace PostQueryInfrastructure.Consumer;

public class PostEventConsumer : IEventConsumer
{
    private readonly MessageBrokerConfig _config;
    private readonly ILogger<PostEventConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public PostEventConsumer(
        IOptions<MessageBrokerConfig> config,
        ILogger<PostEventConsumer> logger, IServiceScopeFactory scopeFactory)
    {
        _config = config.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task Consume()
    {
        try
        {
            await SaveConsume();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while consuming message");
            throw;
        }
    }

    private async Task SaveConsume()
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

        if (_config.ConsumeFromBeginning)
        {
            SetupConsumingFromBeginning(consumer);
        }

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
            
            using (var scope = _scopeFactory.CreateScope())
            {
                IPostEventHandler handler = scope.ServiceProvider.GetRequiredService<IPostEventHandler>();
                var handlerMethod = handler.GetType()
                    .GetMethod(nameof(IPostEventHandler.On), new Type[] { @event.GetType() });
                ArgumentNullException.ThrowIfNull(handlerMethod);
                Task result = (Task)handlerMethod.Invoke(handler, new object[] { @event });
                ArgumentNullException.ThrowIfNull(result);
                await result;
            }

            consumer.Commit(consumeResult);
        }
    }

    private void SetupConsumingFromBeginning(IConsumer<string, string> consumer)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = _config.Hosts
        }).Build();

        // Get the metadata for the topic
        Metadata metadata = adminClient.GetMetadata(_config.TopicName, TimeSpan.FromSeconds(5));
        TopicMetadata topicMetadata = metadata.Topics
            .First(value => value.Topic == _config.TopicName);
        
        // Iterate topic partitions and assign them to the consumer
        foreach (PartitionMetadata partitionMetadata in topicMetadata.Partitions)
        {
            TopicPartitionOffset topicPartitionOffset = new TopicPartitionOffset(
                _config.TopicName, partitionMetadata.PartitionId, Offset.Beginning);
            consumer.Assign(topicPartitionOffset);
        }
    }
}