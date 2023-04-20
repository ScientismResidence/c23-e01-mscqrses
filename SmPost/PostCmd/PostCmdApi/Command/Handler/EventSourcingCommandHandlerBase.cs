using CqrsCore.Handler;
using CqrsCore.Message;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public abstract class EventSourcingCommandHandlerBase<TCommand> : ICommandHandler<TCommand>
    where TCommand : MessageBase
{
    protected EventSourcingCommandHandlerBase(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    {
        EventSourcingHandler = eventSourcingHandler;
    }
    
    protected IEventSourcingHandler<PostAggregate> EventSourcingHandler { get; }

    public abstract Task HandleAsync(TCommand command);
}