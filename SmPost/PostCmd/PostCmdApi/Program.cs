using CqrsCore.Domain;
using CqrsCore.Handler;
using CqrsCore.Infrastructure;
using PostCmdApi;
using PostCmdDomain;
using PostCmdInfrastructure;
using PostCmdInfrastructure.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)))
    .AddScoped<IEventStoreRepository, EventStoreRepository>()
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
app.UseAuthorization();
app.MapControllers();
app.Run();
