using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TF47_Backend.Middleware
{
    public class ApiTokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiTokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
        }
    }
}
