using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using TF47_API.Services.ApiToken;

namespace TF47_API.Middleware
{
    public class ApiTokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiTokenCache _apiTokenCache;

        public ApiTokenAuthenticationMiddleware(RequestDelegate next, ApiTokenCache apiTokenCache)
        {
            _next = next;
            _apiTokenCache = apiTokenCache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey("TF47AuthKey"))
            {
                var isAuthenticated = await _apiTokenCache.IsAuthenticated(httpContext.Request.Headers["TF47AuthKey"]);

                if (isAuthenticated)
                {
                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Api"),
                        new Claim(ClaimTypes.Role, "Api")
                    }));
                }
            }

            await _next(httpContext);
        }
    }
}
