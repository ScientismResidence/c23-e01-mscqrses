using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class UpdateCommentCommandHandler: EventSourcingCommandHandlerBase<UpdateCommentCommand>
{
    public UpdateCommentCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(UpdateCommentCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.UpdateComment(command.CommentId, command.Comment, command.Author);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}