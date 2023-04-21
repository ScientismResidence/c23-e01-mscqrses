using PostQueryApi;
using PostQueryDomain.Repository;
using PostQueryInfrastructure.Handlers;
using PostQueryInfrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddDatabase(builder.Configuration);
builder.Services
    .AddScoped<IPostRepository, PostRepository>()
    .AddScoped<ICommentRepository, CommentRepository>()
    .AddScoped<IPostEventHandler, PostEventHandler>()
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
