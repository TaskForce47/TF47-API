using System.Collections.Generic;
using System.Threading.Tasks;

namespace TF47_API.Services.Authorization
{
    public interface IGroupPermissionCache
    {
        Task RefreshCache();
        Task<bool> CheckPermissionAsync(IEnumerable<string> groups, string requiredPermission);
    }
}