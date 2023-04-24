using CqrsCore.Message;

namespace PostCmdApi.Command;

public class AddCommentCommand : MessageBase
{
    public string Message { get; set; }
    
    public string Author { get; set; }
}