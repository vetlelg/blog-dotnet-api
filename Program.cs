using Microsoft.EntityFrameworkCore;
using blog_dotnet_api.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PostContext>(opt => opt.UseInMemoryDatabase("Post"));
builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("User"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/User", (UserContext context) =>
{
    return context.Users.ToList();
})
.WithName("GetUsers")
.WithOpenApi();

app.MapPost("/Users", (User user, UserContext context) =>
{
    context.Add(user);
    context.SaveChanges();
})
.WithName("CreatePerson")
.WithOpenApi();

app.MapControllers();

app.Run();