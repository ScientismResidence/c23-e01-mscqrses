using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class RemoveCommentCommandHandler : EventSourcingCommandHandlerBase<RemoveCommentCommand>
{
    public RemoveCommentCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(RemoveCommentCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.Author);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}