using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtAuthenticationScheme = builder.Configuration.GetSection("JWTAuthenticationScheme").Get<JWTAuthenticationScheme>();
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAuthenticationScheme?.ValidIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAuthenticationScheme?.ValidAudience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthenticationScheme?.SecretKey)),

        };
    });

// Add Services 
builder.Services.AddSingleton<ITransactionService, TransactionsService>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Adding Configuration service to Model
builder.Services.Configure<ApiCredentials>(builder.Configuration.GetSection("ApiCredentials"));
builder.Services.Configure<JWTAuthenticationScheme>(builder.Configuration.GetSection("JWTAuthenticationScheme"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");

// Middlewares
app.UseMiddleware<BasicAuthMiddleware>();


app.UseHttpsRedirection();

app.UseAuthentication();
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
