using CqrsCore.Message;

namespace PostCmdApi.Command;

public class UpdateCommentCommand : MessageBase
{
    public Guid CommentId { get; set; }
    
    public string Comment { get; set; }
    
    public string Author { get; set; }
}