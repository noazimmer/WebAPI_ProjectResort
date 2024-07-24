using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace BL.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post)
            {
                Log.Information("POST request to {Path}", context.Request.Path);
            }
            else if (context.Request.Method == HttpMethods.Put)
            {
                Log.Information("PUT request to {Path}", context.Request.Path);
            }
            else if (context.Request.Method == HttpMethods.Delete)
            {
                Log.Information("DELETE request to {Path}", context.Request.Path);
            }
            else if (context.Request.Method == HttpMethods.Get)
            {
                Log.Information("GET request to {Path}", context.Request.Path);
            }

            await _next(context);
        }
    }
}
