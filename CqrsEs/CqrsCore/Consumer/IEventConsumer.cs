namespace CqrsCore.Consumer;

public interface IEventConsumer
{
    Task Consume();
}