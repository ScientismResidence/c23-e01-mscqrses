using CqrsCore.Domain;
using CqrsCore.Handler;
using CqrsCore.Infrastructure;
using CqrsCore.Producer;
using PostCmdApi;
using PostCmdDomain;
using PostCmdInfrastructure;
using PostCmdInfrastructure.Config;
using PostCommon.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddMongoDb(builder.Configuration)
    .Configure<MessageBrokerConfig>(builder.Configuration.GetSection(nameof(MessageBrokerConfig)))
    .AddScoped<IEventStoreRepository, EventStoreRepository>()
    .AddScoped<IEventProducer, EventProducer>()
    .AddScoped<IEventStore, EventStore>()
    .AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>()
    .AddCommandHandlers()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "PostCmd service is running...");
});
app.Run();
