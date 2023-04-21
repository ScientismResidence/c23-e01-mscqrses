using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostQueryDomain.Entity;

[Table("Post")]
public class PostEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Message { get; set; }
    
    public string Author { get; set; }
    
    public int Likes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public virtual ICollection<CommentEntity> Comments { get; set; }
}