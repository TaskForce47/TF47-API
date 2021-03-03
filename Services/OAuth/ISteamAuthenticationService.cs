using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TF47_Backend.Services.OAuth
{
    public interface ISteamAuthenticationService
    {
        Task<bool> HandleSteamCallback(HttpContext httpContext);
        Guid CreateChallenge(Guid userId);
    }
}