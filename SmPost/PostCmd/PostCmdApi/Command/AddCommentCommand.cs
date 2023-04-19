using CqrsCore.Message;

namespace PostCmdApi.Command;

public class AddCommentCommand : MessageBase
{
    public string Comment { get; set; }
    
    public string Author { get; set; }
}