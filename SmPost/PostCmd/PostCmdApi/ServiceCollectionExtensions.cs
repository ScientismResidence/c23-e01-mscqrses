using CqrsCore.Infrastructure;
using PostCmdApi.Command;
using PostCmdApi.Command.Handler;
using PostCmdInfrastructure;

namespace PostCmdApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<AddPostCommand>, AddPostCommandHandler>();
        services.AddScoped<ICommandHandler<AddCommentCommand>, AddCommentCommandHandler>();
        services.AddScoped<ICommandHandler<UpdatePostCommand>, UpdatePostCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCommentCommand>, UpdateCommentCommandHandler>();
        services.AddScoped<ICommandHandler<LikePostCommand>, LikePostCommandHandler>();
        services.AddScoped<ICommandHandler<RemoveCommentCommand>, RemoveCommentCommandHandler>();
        services.AddScoped<ICommandHandler<RemovePostCommand>, RemovePostCommandHandler>();
        
        ServiceProvider provider = services.BuildServiceProvider();
        var addPostCommandHandler = provider.GetRequiredService<ICommandHandler<AddPostCommand>>();
        var addCommentCommandHandler = provider.GetRequiredService<ICommandHandler<AddCommentCommand>>();
        var updatePostCommandHandler = provider.GetRequiredService<ICommandHandler<UpdatePostCommand>>();
        var updateCommentCommandHandler = provider.GetRequiredService<ICommandHandler<UpdateCommentCommand>>();
        var likePostCommandHandler = provider.GetRequiredService<ICommandHandler<LikePostCommand>>();
        var removeCommentCommandHandler = provider.GetRequiredService<ICommandHandler<RemoveCommentCommand>>();
        var removePostCommandHandler = provider.GetRequiredService<ICommandHandler<RemovePostCommand>>();

        ICommandDispatcher dispatcher = new CommandDispatcher();
        dispatcher.RegisterHandler<AddPostCommand>(addPostCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<AddCommentCommand>(addCommentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdatePostCommand>(updatePostCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<UpdateCommentCommand>(updateCommentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<LikePostCommand>(likePostCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<RemoveCommentCommand>(removeCommentCommandHandler.HandleAsync);
        dispatcher.RegisterHandler<RemovePostCommand>(removePostCommandHandler.HandleAsync);

        services.AddSingleton(_ => dispatcher);
        
        return services;
    }
}