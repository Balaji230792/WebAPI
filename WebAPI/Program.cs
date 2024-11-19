using Microsoft.Extensions.Logging;
using WebAPI.Middlewares;
using WebAPI.Models;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Logging.AddConsole();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Services 
builder.Services.AddSingleton<ITransactionService, TransactionsService>();

// Adding Configuration service to Model
builder.Services.Configure<ApiCredentials>(builder.Configuration.GetSection("ApiCredentials"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseMiddleware<BasicAuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/", (ILogger<Program> logger) =>
//{
//    logger.LogInformation("This is an Information log");
//    logger.LogWarning("This is a Warning log");
//    logger.LogError("This is an Error log");
//    return "Check your logs!";
//});
app.Run();
