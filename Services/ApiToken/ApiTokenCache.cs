using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Services.ApiToken
{
    public class ApiTokenCache
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApiTokenCache> _logger;
        private readonly Dictionary<string, ApiKey> _cache;
        private bool _updateLock;

        public ApiTokenCache(IServiceProvider serviceProvider, ILogger<ApiTokenCache> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _cache = new Dictionary<string, ApiKey>();
            _updateLock = false;

#pragma warning disable 4014
            BackgroundTask();
#pragma warning restore 4014
        }

        public async Task<bool> IsAuthenticated(string apiKey)
        {

            if (_updateLock)
                await Task.Run(async () =>
                {
                    while (_updateLock)
                    {
                        await Task.Delay(500);
                    }
                });



            if (!_cache.TryGetValue(apiKey, out var result)) 
                return false;
            if (result.ValidUntil >= DateTime.Now) 
                return true;

            _logger.LogWarning($"Someone tried to access using expired api key");
            return false;

        }

        public async Task BackgroundTask()
        {
            while (true)
            {
                using var scope = _serviceProvider.CreateScope();
                await using var database = _serviceProvider.GetService<DatabaseContext>();
                var apiKeys = database?.ApiKeys.Where(x => x.ValidUntil > DateTime.Now);

                _updateLock = true;
                _cache.Clear();

                if (apiKeys != null)
                    foreach (var apiKey in apiKeys)
                    {
                        _cache.Add(apiKey.ApiKeyValue, apiKey);
                    }

                _updateLock = false;

                await Task.Delay(1000 * 60);
            }
        }

    }
}
