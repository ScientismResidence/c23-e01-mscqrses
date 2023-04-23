using Microsoft.EntityFrameworkCore;
using PostQueryInfrastructure;

namespace PostQueryApi;

public static class ServiceCollectionExtensions
{
    public static async Task<IServiceCollection> AddDatabase(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        Action<DbContextOptionsBuilder> configureDbContext = (options) =>
            options
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("Default"));

        services
            .AddDbContext<SocialMediaDbContext>(configureDbContext)
            .AddSingleton(new SocialMediaDbContextFactory(configureDbContext));

        IServiceProvider provider = services.BuildServiceProvider();
        await using DbContext context = provider.GetRequiredService<SocialMediaDbContext>();

        await context.Database.EnsureCreatedAsync();
        
        return services;
    }
}