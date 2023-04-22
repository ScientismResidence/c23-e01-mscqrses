using CqrsCore.Consumer;
using PostQueryApi;
using PostQueryDomain.Repository;
using PostQueryInfrastructure;
using PostQueryInfrastructure.Config;
using PostQueryInfrastructure.Consumer;
using PostQueryInfrastructure.Handlers;
using PostQueryInfrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<MessageBrokerConfig>(builder.Configuration.GetSection(nameof(MessageBrokerConfig)));

await builder.Services.AddDatabase(builder.Configuration);

builder.Services
    .AddScoped<IPostRepository, PostRepository>()
    .AddScoped<ICommentRepository, CommentRepository>()
    .AddScoped<IPostEventHandler, PostEventHandler>()
    .AddScoped<IEventConsumer, PostEventConsumer>()
    .AddHostedService<ConsumerHostedService>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
