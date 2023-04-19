using CqrsCore.Message;

namespace PostCmdApi.Command;

public class AddPostCommand : MessageBase
{
    public string Author { get; set; }
    
    public string Message { get; set; }
}