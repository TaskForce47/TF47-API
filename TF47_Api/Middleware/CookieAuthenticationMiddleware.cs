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
            if (httpContext.Request.Path.Value == "/api/user/authenticate") 
                await _next(httpContext);
            else
            {
                if (httpContext.Request.Cookies.ContainsKey("express.sid"))
                {
                    var cookie = httpContext.Request.Cookies["express.sid"];
                    var user = _authenticationProvider.GetClaimsPrincipal(cookie);
                    if (user == null)
                    {
                        _logger.LogInformation($"{httpContext.Connection.LocalIpAddress} not logged in!");
                        httpContext.Response.Clear();
                        httpContext.Response.StatusCode = 403;
                        await httpContext.Response.WriteAsync("not logged in!");
                        return;
                    }

                    httpContext.User = user;
                }
                else
                {
                    _logger.LogInformation($"{httpContext.Connection.LocalIpAddress} no foum cookie found!");
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = 403;
                    await httpContext.Response.WriteAsync("no cookie found");
                    return;
                }

                await _next(httpContext);
            }
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