using System.Linq;
using System.Net;
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
                if (path.Contains(allowedUrl) || path == allowedUrl) return true;
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
                    var (authenticationStatus, claimsPrincipal) = await _authenticationProvider.IsUserAuthenticatedAsync(cookie);
                    switch (authenticationStatus)
                    {
                        case AuthenticationProviderService.AuthenticationStatus.BadRequest:
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await httpContext.Response.WriteAsync($"no cookie found in request.");
                                return;
                        }
                        case AuthenticationProviderService.AuthenticationStatus.Expired:
                        {
                            httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            await httpContext.Response.WriteAsync("session is expired.");
                            return;
                        }
                        case AuthenticationProviderService.AuthenticationStatus.LoggedOut:
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
                            httpContext.Response.Redirect("/api/user/authenticate");
                            return;
                        }
                        case AuthenticationProviderService.AuthenticationStatus.LoggedIn:
                        {
                            break;
                        }
                    }
                    httpContext.User = claimsPrincipal;
                }
                else
                {
                    _logger.LogInformation($"{httpContext.Connection.LocalIpAddress} no forum cookie found!");
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