using CqrsCore.Message;

namespace PostCmdApi.Command;

public class EditCommentCommand : MessageBase
{
    public Guid CommentId { get; set; }
    
    public string Comment { get; set; }
    
    public string Author { get; set; }
}