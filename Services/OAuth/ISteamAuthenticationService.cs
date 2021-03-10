using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TF47_Backend.Services.OAuth
{
    public interface ISteamAuthenticationService
    {
        Task<SteamUserResponse> HandleSteamCallbackAsync(HttpContext httpContext);
        string CreateChallenge(HttpContext httpContext, string callbackPath);
    }
}