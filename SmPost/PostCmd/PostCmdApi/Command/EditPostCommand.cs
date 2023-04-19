using CqrsCore.Message;

namespace PostCmdApi.Command;

public class EditPostCommand : MessageBase
{
    public string Message { get; set; }
}