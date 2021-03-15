using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Services
{
    public interface IUserProviderService
    {
        Task<User> GetDatabaseUser(HttpContext context);
    }
}