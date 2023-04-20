using CqrsCore.Message;

namespace PostCmdApi.Command;

public class UpdatePostCommand : MessageBase
{
    public string Message { get; set; }
}