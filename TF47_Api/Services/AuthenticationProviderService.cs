using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Api.Models;

namespace TF47_Api.Services
{
    public class AuthenticationProviderService
    {
        private readonly ILogger<AuthenticationProviderService> _logger;
        private readonly IServiceProvider _serverProvider;
        private readonly Dictionary<string, AuthenticationUserData> _users;

        public AuthenticationProviderService(ILogger<AuthenticationProviderService> logger,IServiceProvider serverProvider)
        {
            _logger = logger;
            _serverProvider = serverProvider;
            _users = new Dictionary<string, AuthenticationUserData>();
        }

        public async Task<ClaimsPrincipal> AuthenticateUserAsync(string cookie)
        {
            if (_users.ContainsKey(cookie))
            {
                var userData = _users[cookie];
                if (userData.ExpirationDate < DateTime.Now)
                {
                    var user = await GetClaimsPrincipalAsync(cookie);
                    _users.Remove(cookie);
                    if (user == null) return null;
                    _users.Add(cookie, new AuthenticationUserData
                    {
                        ClaimsPrincipal = user,
                        ExpirationDate = DateTime.Now.AddHours(2)
                    });
                    return user;
                }
                return userData.ClaimsPrincipal;
            }
            else
            {
                var user = await GetClaimsPrincipalAsync(cookie);
                if (user == null)
                {
                    _users.Remove(cookie);
                    return null;
                }
                var data = new AuthenticationUserData
                {
                    ClaimsPrincipal = user,
                    ExpirationDate = DateTime.Now + TimeSpan.FromHours(2)
                };
                _users.Add(cookie, data);
                return data.ClaimsPrincipal;
            }
        }

        private async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string cookie)
        {
            _logger.LogInformation("Requesting new claims principal data");
            using var scope = _serverProvider.CreateScope();
            var forumDataProvider = scope.ServiceProvider.GetRequiredService<ForumDataProviderService>();
            var claimProviderService = scope.ServiceProvider.GetRequiredService<ClaimProviderService>();
            var user = await forumDataProvider.GetUserInfoAsync(cookie);
            if (user != null)
                return await claimProviderService.GetClaimsPrincipalAsync(user);
            else
                return null;
        }
    }
}
