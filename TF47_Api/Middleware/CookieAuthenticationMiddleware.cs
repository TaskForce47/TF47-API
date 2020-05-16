using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TF47_Api.Services;

namespace TF47_Api.Middleware
{
    public class CookieAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<CookieAuthenticationMiddleware> _logger;
        private readonly AuthenticationProviderService _authenticationProvider;

        public CookieAuthenticationMiddleware(RequestDelegate next, ILogger<CookieAuthenticationMiddleware> logger, AuthenticationProviderService authenticationProvider)
        {
            _next = next;
            _logger = logger;
            _authenticationProvider = authenticationProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey("express.sid"))
            {
                var cookie = httpContext.Request.Cookies["express.sid"];
                var user = await _authenticationProvider.AuthenticateUserAsync(cookie);
                if (user == null)
                {
                    _logger.LogInformation($"{httpContext.Connection.LocalIpAddress} no forum data found!");
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = 403;
                    await httpContext.Response.WriteAsync("not authorized");
                    return;
                }

                httpContext.User = user;
            }
            else
            {
                _logger.LogInformation($"{httpContext.Connection.LocalIpAddress} no cookie found!");
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsync("no cookie found");
                return;
            }
            await _next(httpContext);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomCookieAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieAuthenticationMiddleware>();
        }
    }
}