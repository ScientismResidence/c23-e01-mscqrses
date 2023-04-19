using CqrsCore.Infrastructure;
using CqrsCore.Message;

namespace PostCmdInfrastructure;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<MessageBase, Task>> _handlers = new();

    public void RegisterHandler<TCommand>(Func<TCommand, Task> handler) where TCommand : MessageBase
    {
        if (_handlers.ContainsKey(typeof(TCommand)))
        {
            throw new ArgumentException($"Handler for {typeof(TCommand).Name} already registered");
        }
        
        _handlers.Add(typeof(TCommand), command => handler((TCommand) command));
    }

    public Task SendAsync(MessageBase command)
    {
        if (!_handlers.TryGetValue(command.GetType(), out Func<MessageBase, Task> handler))
        {
            throw new ArgumentException($"Handler for {command.GetType().Name} not registered");
        }
        
        return handler(command);
    }
}