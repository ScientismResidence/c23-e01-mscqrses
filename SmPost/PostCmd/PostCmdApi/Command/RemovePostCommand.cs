using CqrsCore.Message;

namespace PostCmdApi.Command;

public class RemovePostCommand : MessageBase
{
    public string Author { get; set; }
}