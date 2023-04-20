using CqrsCore.Handler;
using PostCmdDomain;

namespace PostCmdApi.Command.Handler;

public class AddPostCommandHandler : EventSourcingCommandHandlerBase<AddPostCommand>
{
    public AddPostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        : base(eventSourcingHandler)
    {
    }

    public override async Task HandleAsync(AddPostCommand command)
    {
        PostAggregate aggregate = new PostAggregate(command.Id, command.Author, command.Author);
        await EventSourcingHandler.SaveAsync(aggregate);
    }
}