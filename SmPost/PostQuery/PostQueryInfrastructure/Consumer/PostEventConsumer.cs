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
            IConsumer<string, string> consumer = ConfigureConsumer();
            await StartConsumptionAsync(consumer);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while consuming message");
            throw;
        }
    }

    private IConsumer<string, string> ConfigureConsumer()
    {
        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = _config.Hosts,
            GroupId = _config.ConsumerConfig.GroupId,
            AutoOffsetReset = (AutoOffsetReset)_config.ConsumerConfig.AutoOffsetReset,
            EnableAutoCommit = _config.ConsumerConfig.EnableAutoCommit,
            AllowAutoCreateTopics = _config.ConsumerConfig.AllowAutoCreateTopics
        };

        ConsumerBuilder<string, string> builder = new ConsumerBuilder<string, string>(config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8);

        if (_config.ConsumeFromBeginning)
        {
            builder = builder.SetPartitionsAssignedHandler((_, partitions) =>
            {
                return partitions
                    .Select(partition => new TopicPartitionOffset(partition, Offset.Beginning))
                    .ToList();
            });
        }

        IConsumer<string, string> consumer = builder.Build();
        consumer.Subscribe(_config.TopicName);

        return consumer;
    }

    private async Task StartConsumptionAsync(IConsumer<string, string> consumer)
    {
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
}