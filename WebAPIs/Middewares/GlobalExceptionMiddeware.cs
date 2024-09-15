using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebAPIs.Middewares
{
    public class GlobalExceptionMiddeware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddeware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message
                };
                string json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
