using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class UpdatePostCommandHandler : EventSourcingCommandHandlerBase<UpdatePostCommand>
{
    public UpdatePostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(UpdatePostCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.UpdatePost(command.Message);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}