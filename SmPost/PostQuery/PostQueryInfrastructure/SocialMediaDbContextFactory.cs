using Microsoft.EntityFrameworkCore;

namespace PostQueryInfrastructure;

public class SocialMediaDbContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;
    
    public SocialMediaDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }
    
    public SocialMediaDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<SocialMediaDbContext> optionsBuilder = new();
        _configureDbContext(optionsBuilder);
        return new SocialMediaDbContext(optionsBuilder.Options);
    }
}