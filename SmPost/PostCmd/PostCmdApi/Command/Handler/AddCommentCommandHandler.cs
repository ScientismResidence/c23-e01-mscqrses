using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class AddCommentCommandHandler : EventSourcingCommandHandlerBase<AddCommentCommand>
{
    public AddCommentCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(AddCommentCommand command)
    {
        PostAggregate aggregate = await EventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.AddComment(command.Author, command.Message);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}