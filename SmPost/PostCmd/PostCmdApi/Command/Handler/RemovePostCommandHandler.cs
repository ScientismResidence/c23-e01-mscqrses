using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class RemovePostCommandHandler : EventSourcingCommandHandlerBase<RemovePostCommand>
{
    public RemovePostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(RemovePostCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemovePost(command.Author);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}