using System;
using System.Threading.Tasks;

namespace TF47_Backend.Services.Authentication
{
    public interface IUserManager
    {
        ITokenProvider GetTokenProvider();
        Task<UserManager.AuthenticatedUser> AuthenticateUser(string username, string password);
        Task<UserManager.AuthenticatedUser> CreateUser(string username, string password, string email);
        Task<bool> DeleteUser(Guid id);
    }
}