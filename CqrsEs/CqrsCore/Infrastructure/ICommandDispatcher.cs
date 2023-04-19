using CqrsCore.Message;

namespace CqrsCore.Infrastructure;

public interface ICommandDispatcher
{
    void RegisterHandler<TCommand>(Func<TCommand, Task> handler) where TCommand : MessageBase;
    
    Task SendAsync(MessageBase command);
}