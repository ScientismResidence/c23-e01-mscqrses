using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PostQueryDomain.Entity;

[Table("Comment")]
public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }
    
    public Guid PostId { get; set; }
    
    public string Author { get; set; }
    
    public string Message { get; set; }
    
    public bool IsEdited { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public virtual PostEntity Post { get; set; }
}