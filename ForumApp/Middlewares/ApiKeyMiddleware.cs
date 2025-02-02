using ForumApp.Models;
using System.Text.Json;

namespace ForumApp.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey"];
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-API-KEY") ||
                context.Request.Headers["X-API-KEY"] != _apiKey)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    Message = "Forbidden: Invalid API key",
                    StatusCode = 403
                };
                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
                return;
            }
            await _next(context); 
        }
    }
}
