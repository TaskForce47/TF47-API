using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TF47_API.Services.OAuth;

namespace TF47_API.Services.Authentication
{
    public interface IAuthenticationManager
    {
        Task<ClaimsIdentity> UpdateOrCreateUserAsync(Player steamUser);
        Task<ClaimsIdentity> GetClaimIdentityAsync(Guid guid);
    }
}
