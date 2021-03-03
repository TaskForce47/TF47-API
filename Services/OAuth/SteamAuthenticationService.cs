using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Services.Authentication;

namespace TF47_Backend.Services.OAuth
{
    public class SteamAuthenticationService : ISteamAuthenticationService
    {
        private readonly ILogger<SteamAuthenticationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Guid, Guid> _steamChallenges;


        public SteamAuthenticationService(ILogger<SteamAuthenticationService> logger, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _steamChallenges = new Dictionary<Guid, Guid>();
        }

        public async Task<bool> HandleSteamCallback(HttpContext httpContext)
        {
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetService<DatabaseContext>();


            _logger.LogInformation($"Callback guid: {httpContext.Request.Path}");

            var guid = Guid.Parse(httpContext.Request.Path.Value.Replace("/api/user/steamCallback/", ""));

            string queryString;
            if (httpContext.Request.QueryString.Value != null)
            {
                queryString = httpContext.Request.QueryString.Value.Replace("&openid.mode=id_res",
                    "&openid.mode=check_authentication");
            }
            else
            {
                _logger.LogWarning("Invalid query string in steam challenge response");
                return false;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync($"https://steamcommunity.com/openid/login{queryString}");

            if (!response.Contains("is_valid:true"))
            {
                _logger.LogWarning($"Challenge {guid} is not valid!");
                return false;
            }

            var userId = _steamChallenges[guid];
            var user = database?.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                _logger.LogWarning("Challenge user not found");
                return false;
            }

            var steamId = httpContext.Request.Query.First(x => x.Key == "openid.identity").Value
                .ToString()
                .Replace("https://steamcommunity.com/openid/id/", "");


            user.SteamId = steamId;
            user.IsConnectedSteam = true;
            await database.SaveChangesAsync();

            _steamChallenges.Remove(guid);

            return true;

        }

        public Guid CreateChallenge(Guid userId)
        {
            var guid = Guid.NewGuid();
            _steamChallenges.Add(guid, userId);
            return guid;
        }
    }
}
