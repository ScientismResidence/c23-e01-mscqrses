using Microsoft.EntityFrameworkCore;
using PostQueryDomain.Entity;

namespace PostQueryInfrastructure;

public class SocialMediaDbContext : DbContext
{
    public SocialMediaDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<PostEntity> Posts { get; set; }
    
    public DbSet<CommentEntity> Comments { get; set; }
}