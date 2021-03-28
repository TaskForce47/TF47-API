using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp.Serialization.Json;
using TF47_API.Database;
using TF47_API.Services.Authentication;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TF47_API.Services.OAuth
{
    public class SteamAuthenticationService : ISteamAuthenticationService
    {
        private readonly ILogger<SteamAuthenticationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<Guid> _steamChallenges;
        private readonly string _apiToken;

        public SteamAuthenticationService(ILogger<SteamAuthenticationService> logger,
            IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _steamChallenges = new List<Guid>();
            _apiToken = configuration["Credentials:Steam:ApiToken"];
        }

        public async Task<object> HandleCallbackAsync(HttpContext httpContext)
        {
            _logger.LogInformation($"Callback guid: {httpContext.Request.Path}");

            var challengeGuidString = httpContext.Request.Cookies.FirstOrDefault(x => x.Key == "tf47_steam_challenge").Value;
            if (string.IsNullOrEmpty(challengeGuidString))
            {
                _logger.LogWarning($"Got steam callback without guid cookie set!");
                return null;
            }

            var challengeGuid = Guid.Parse(challengeGuidString);

            //begin steam verification challenge
            string queryString;
            if (httpContext.Request.QueryString.Value != null)
            {
                queryString = httpContext.Request.QueryString.Value.Replace("&openid.mode=id_res",
                    "&openid.mode=check_authentication");
            }
            else
            {
                _logger.LogWarning("Invalid query string in steam challenge response");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync($"https://steamcommunity.com/openid/login{queryString}");

            if (!response.Contains("is_valid:true"))
            {
                _logger.LogWarning($"Challenge {challengeGuid} is not valid!");
                return null;
            }
            //end steam verification challenge

            var userId = _steamChallenges.FirstOrDefault(x => x == challengeGuid);

            var steamId = httpContext.Request.Query.First(x => x.Key == "openid.identity").Value
                .ToString()
                .Replace("https://steamcommunity.com/openid/id/", "");

            _steamChallenges.Remove(challengeGuid);
            //query steam user from api


            response = await client.GetStringAsync(
                $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_apiToken}&steamids={steamId}");
            var steamUser = JsonSerializer.Deserialize<SteamUserResponse>(response);

            return steamUser;
        }

        //Generates steam oath url with challenge build in
        public string CreateChallenge(HttpContext httpContext, string callbackPath)
        {
            var challengeGuid = Guid.NewGuid();
            _steamChallenges.Add(challengeGuid);
            httpContext.Response.Cookies.Append("tf47_steam_challenge", challengeGuid.ToString(), new CookieOptions
            {
                Domain = httpContext.Request.Host.Host,
                Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(5),
                HttpOnly = true,
                IsEssential = true,
                Path = "/",
                Secure = true
            });
            return $"https://steamcommunity.com/openid/login?" + $"{GetHeaders(httpContext, callbackPath)}";
        }


        private static string GetHeaders(HttpContext httpContext, string callbackPath)
        {
            var headers = new Dictionary<string, string>
            {
                {"openid.ns", "http://specs.openid.net/auth/2.0"},
                {"openid.mode", "checkid_setup"},
                {"openid.return_to", $"https://{httpContext.Request.Host.Value}/{callbackPath}"},
                {"openid.realm", $"https://{httpContext.Request.Host.Value}"},
                {"openid.identity", "http://specs.openid.net/auth/2.0/identifier_select"},
                {"openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select"}
            };
            foreach (var (key, value) in headers)
            {

                headers[key] = value.Replace(":", "%3A").Replace("/", "%2F");
            }

            return string.Join("&", headers.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

    }
}
