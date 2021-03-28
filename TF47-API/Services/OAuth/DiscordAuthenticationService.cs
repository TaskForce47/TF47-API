using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TF47_API.Services.OAuth
{
    public class DiscordAuthenticationService : IDiscordAuthenticationService
    {
        private readonly ILogger<DiscordAuthenticationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _callbackUrl;

        public DiscordAuthenticationService(ILogger<DiscordAuthenticationService> logger,
            IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _clientId = configuration["Credentials:Discord:ClientId"];
            _clientSecret = configuration["Credentials:Discord:ClientSecret"];
            _callbackUrl = configuration["Credentials:Discord:CallbackUrl"];
        }
            
        public async Task<object> HandleCallbackAsync(HttpContext httpContext)
        {
            _logger.LogInformation($"Callback guid: {httpContext.Request.Path}");

            string code;
            if (httpContext.Request.QueryString.Value != null)
                code = httpContext.Request.QueryString.Value.Replace("?code=", "");
            else
            {
                _logger.LogWarning("Invalid query string in discord challenge response");
                return null;
            }

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://discord.com/api/oauth2/token")
            {
                Content = GetFormEncodedAccessTokenExchangeRequest(code)
            };
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to obtain jwt token, Statuscode: {response.StatusCode}, Message: {await response.Content.ReadAsStringAsync()}");
                return null;
            }
            var accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(await response.Content.ReadAsStringAsync());

            try
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/Json"));

                var userResponse =
                    JsonSerializer.Deserialize<DiscordUserResponse>(
                        await client.GetStringAsync("https://discordapp.com/api/users/@me"));

                return userResponse;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to bind discord result: {ex.Message}");
                return null;
            }
        }

        public string CreateChallenge(HttpContext httpContext, string callbackPath)
        {
            return $"https://discord.com/api/oauth2/authorize?{GetFormEncodedOauthRequest(httpContext)}";
        }

        private string GetFormEncodedOauthRequest(HttpContext httpContext)
        {
            var data = new Dictionary<string, string>
            {
                {"client_id", _clientId },
                {"redirect_uri", _callbackUrl},
                {"response_type", "code"},
                {"scope", "identify email connections guilds"}
            };
            foreach (var (key, value) in data)
            {

                data[key] = value.Replace(":", "%3A").Replace("/", "%2F").Replace(" ", "%20");
            }

            return string.Join("&", data.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
        private FormUrlEncodedContent GetFormEncodedAccessTokenExchangeRequest(string code)
        {
            var data = new Dictionary<string, string>
            {
                {"client_id", _clientId },
                {"client_secret", _clientSecret },
                {"grant_type", "authorization_code"},
                {"redirect_uri", _callbackUrl},
                {"code", code},
                {"scope", "identify email connections guilds"}
            };
            return new FormUrlEncodedContent(data);
        }

        private class AccessTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }

            [JsonPropertyName("expirese")]
            public int Expirese { get; set; }

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonPropertyName("scope")]
            public string Scope { get; set; }
        }
    }
}
