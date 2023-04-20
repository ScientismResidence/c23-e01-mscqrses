using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class LikePostCommandHandler : EventSourcingCommandHandlerBase<LikePostCommand>
{
    public LikePostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(LikePostCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.LikePost();
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}