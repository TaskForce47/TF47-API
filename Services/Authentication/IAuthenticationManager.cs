using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TF47_Backend.Services.OAuth;

namespace TF47_Backend.Services.Authentication
{
    public interface IAuthenticationManager
    {
        Task<ClaimsIdentity> CreateUserAsync(Player steamUser);
        Task UpdateUserDataAsync(Player steamUser);
        Task<ClaimsIdentity> GetClaimIdentityAsync(Guid guid);
    }
}