using CqrsCore.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PostQueryInfrastructure;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _provider;

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer hosted service started");
        
        using IServiceScope scope = _provider.CreateScope();
        IEventConsumer consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
        
        Task.Run(() => consumer.Consume(), cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer hosted service stopped");
        
        return Task.CompletedTask;
    }
}