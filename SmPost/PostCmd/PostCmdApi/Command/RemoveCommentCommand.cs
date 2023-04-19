using CqrsCore.Message;

namespace PostCmdApi.Command;

public class RemoveCommentCommand : MessageBase
{
    public Guid CommentId { get; set; }
    
    public string Author { get; set; }
}