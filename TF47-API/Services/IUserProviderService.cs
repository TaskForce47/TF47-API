using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TF47_API.Database.Models.Services;

namespace TF47_API.Services
{
    public interface IUserProviderService
    {
        Task<User> GetDatabaseUser(HttpContext context);
    }
}
