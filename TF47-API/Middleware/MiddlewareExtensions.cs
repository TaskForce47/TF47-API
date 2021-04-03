using Microsoft.AspNetCore.Builder;

namespace TF47_API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiTokenAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiTokenAuthenticationMiddleware>();
        }
    }
}