using PostQueryApi;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
