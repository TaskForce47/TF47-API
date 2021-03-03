using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TF47_Backend.Services.Authentication
{
    public interface ITokenProvider
    {
        Task<string> GenerateToken(Guid guid);
        Task<ClaimsIdentity> GetClaimsIdentity(Guid guid);
    }
}