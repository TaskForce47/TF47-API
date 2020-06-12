using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TF47_Api.Services;

namespace TF47_Api.Middleware
{
    public class CookieAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CookieAuthenticationMiddleware> _logger;
        private readonly AuthenticationProviderService _authenticationProvider;
        private readonly string[] _allowedUrls;

        public CookieAuthenticationMiddleware(RequestDelegate next, ILogger<CookieAuthenticationMiddleware> logger, AuthenticationProviderService authenticationProvider, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _authenticationProvider = authenticationProvider;
            _allowedUrls = configuration.GetSection("CookieBypassPaths").Get<string[]>();
        }

        private bool IsAllowedUrl(string path)
        {
            foreach (var allowedUrl in _allowedUrls)
            {
                if (allowedUrl.Contains(path)) return true;
            }

            return false;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (IsAllowedUrl(httpContext.Request.Path.Value)) 
                await _next(httpContext);
            else
            {
                if (httpContext.Request.Cookies.ContainsKey("express.sid"))
                {
                    var cookie = httpContext.Request.Cookies["express.sid"];
                    var user = await _authenticationProvider.AuthenticateUserAsync(cookie);
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