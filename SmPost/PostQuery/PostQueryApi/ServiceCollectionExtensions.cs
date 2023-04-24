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
    private const string MsSqlConnection = "MsSql";
    private const string PostgresConnection = "Postgres";
    
    public static async Task<IServiceCollection> AddDatabase(
        this IServiceCollection services, ConfigurationManager configuration)
    {
        void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies();
            string connectionString = configuration.GetValue<string>("ConnectionString");
            switch (connectionString)
            {
                case MsSqlConnection:
                    options.UseSqlServer(configuration.GetConnectionString(MsSqlConnection));
                    break;
                case PostgresConnection:
                    options.UseNpgsql(configuration.GetConnectionString(PostgresConnection));
                    break;
                default:
                    throw new ArgumentException(
                        "Unsupported database type. " + 
                        "Specify supported database type in configuration file.");
            }
        }

        services
            .AddDbContext<SocialMediaDbContext>((Action<DbContextOptionsBuilder>)ConfigureDbContext)
            .AddSingleton(new SocialMediaDbContextFactory(ConfigureDbContext));

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