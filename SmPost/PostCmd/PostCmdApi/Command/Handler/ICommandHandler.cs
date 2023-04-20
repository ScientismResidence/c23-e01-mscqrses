using CqrsCore.Message;

namespace PostCmdApi.Command.Handler;

public interface ICommandHandler<in TCommand> where TCommand : MessageBase
{
    Task HandleAsync(TCommand command);
}