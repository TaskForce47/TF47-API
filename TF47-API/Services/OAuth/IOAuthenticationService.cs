using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TF47_API.Services.OAuth
{
    public interface IOAuthenticationService
    {
        public Task<object> HandleCallbackAsync(HttpContext httpContext);
        public string CreateChallenge(HttpContext context, string callbackPath);
    }

    public interface ISteamAuthenticationService : IOAuthenticationService { }
    public interface IDiscordAuthenticationService : IOAuthenticationService { }
}
