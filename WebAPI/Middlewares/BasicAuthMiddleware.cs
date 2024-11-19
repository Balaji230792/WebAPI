using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Middlewares
{
    public class BasicAuthMiddleware(RequestDelegate next, IOptions<ApiCredentials> apiCredentials)
    {
        private readonly RequestDelegate _next = next;
        private readonly ApiCredentials _apiCredentials = apiCredentials.Value;
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", System.StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid authorization header.");
                return;
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(decodedBytes).Split(':', 2);

            if (credentials.Length != 2 || !IsAuthorized(credentials[0], credentials[1]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            await _next(context);
        }

        private bool IsAuthorized(string userName, string password)
        {
            return userName == _apiCredentials.UserName && password == _apiCredentials.Password;
        }
    }
}
