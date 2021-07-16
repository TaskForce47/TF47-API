using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;

namespace TF47_API.Services.Authorization
{
    public class GroupPermissionCache : IGroupPermissionCache
    {
        private readonly ILogger<GroupPermissionCache> _logger;
        private readonly IServiceProvider _serviceProvider;

        private bool _updateLock;
        private readonly Dictionary<string, ICollection<string>> _permissionCache;
        
        public GroupPermissionCache(
            ILogger<GroupPermissionCache> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _permissionCache = new Dictionary<string, ICollection<string>>();
            _updateLock = false;
            RefreshCache().Wait();

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(60*1000);
                    await RefreshCache();
                }
            });
        }

        public async Task RefreshCache()
        {
            _updateLock = true;
            _logger.LogInformation("Updating permission cache"); 
            await Task.Delay(10);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                await using var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var groups = database.Groups
                    .Include(x => x.Permissions);

                _permissionCache.Clear();
                foreach (var group in groups)
                {
                    _permissionCache.Add(group.Name, group.Permissions.Select(x => x.Name).ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update permission cache! {message}", ex.Message);
            }
            finally
            {
                _updateLock = false;
            }
            
            _logger.LogInformation("Updating permission cache completed");
            await Task.CompletedTask;
        }

        public async Task<bool> CheckPermissionAsync(IEnumerable<string> groups, string requiredPermission)
        {
            if (!groups.Any()) return false;
            
            if (_updateLock)
            {
                while (_updateLock)
                {
                    await Task.Delay(5);
                }
            }

            foreach (var group in groups)
            {
                if (! _permissionCache.ContainsKey(group))
                {
                    _logger.LogWarning("User has group {groupName} that does not exist in the cache!", group);
                    continue;
                }

                if (_permissionCache[group].Contains(requiredPermission))
                {
                    return true;
                }
            }
            
            return false;
        }
        
    }
}