using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TF47_Api.Middleware
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorsMiddleware> _logger;

        public CorsMiddleware(RequestDelegate next, ILogger<CorsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var origin = context.Request.Headers["origin"];
            _logger.LogInformation(context.Request.Method);
            _logger.LogInformation(origin);
            
            context.Response.Headers.Add("Access-Control-Allow-Origin", "https://gadget.taskforce47.com");
            
            context.Response.StatusCode = 200;
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Accept-Encoding, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS");
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(string.Empty);
                return;
            }

            await _next(context);
        }
    }

    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}