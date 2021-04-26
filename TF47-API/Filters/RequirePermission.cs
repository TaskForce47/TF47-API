using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TF47_API.Services.Authorization;

namespace TF47_API.Filters
{
    public class RequirePermission : Attribute, IAsyncActionFilter
    {
        private readonly string _requiredPermission;

        public RequirePermission(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers.Any(x => x.Key == "TF47AuthKey"))
            {
                await next();
                return;
            }
            
            var groupPermissionCache = context.HttpContext.RequestServices.GetRequiredService<IGroupPermissionCache>();

            var roles = context.HttpContext.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value);

            var isAuthorized = await groupPermissionCache.CheckPermissionAsync(roles, _requiredPermission);

            if (isAuthorized)
            {
                await next();
            }
            else
            {
                var response = JsonConvert.SerializeObject(new
                {
                    Method = context.HttpContext.Request.Method,
                    Path = context.HttpContext.Request.Path,
                    Message = $"This endpoint requires the {_requiredPermission} permission set."
                });
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync(response, Encoding.UTF8, CancellationToken.None);
                
                // ReSharper disable once RedundantJumpStatement
                return;
            }
        }
    }
}