using CqrsCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PostQueryApi.Handler;
using PostQueryApi.Query;
using PostQueryDomain.Entity;
using PostQueryInfrastructure;
using PostQueryInfrastructure.Dispatcher;

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
    
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler, QueryHandler>();
        
        IServiceProvider provider = services.BuildServiceProvider();
        IQueryHandler handler = provider.GetRequiredService<IQueryHandler>();
        
        var dispatcher = new QueryDispatcher();
        dispatcher.RegisterHandler<GetAllPostsQuery>(handler.HandleAsync);
        dispatcher.RegisterHandler<GetPostByIdQuery>(handler.HandleAsync);
        dispatcher.RegisterHandler<GetPostsByAuthorQuery>(handler.HandleAsync);
        dispatcher.RegisterHandler<GetPostsWithCommentsQuery>(handler.HandleAsync);
        dispatcher.RegisterHandler<GetPostsWithLikesQuery>(handler.HandleAsync);
        
        services.AddSingleton<IQueryDispatcher<PostEntity>>(dispatcher);

        return services;
    }
}