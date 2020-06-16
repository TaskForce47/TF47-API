using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using TF47_Api.Models;

namespace TF47_Api.Services
{
    public class ForumDataProviderService
    {
        private readonly ILogger<ForumDataProviderService> _logger;

        public ForumDataProviderService(ILogger<ForumDataProviderService> logger)
        {
            _logger = logger;
        }

        public async Task<ForumUser> GetUserInfoAsync(string cookieValue)
        {
            var client = new RestClient("https://forum.taskforce47.com/") {CookieContainer = new CookieContainer()};
            client.CookieContainer.Add(new Cookie("express.sid", cookieValue, "/", ".taskforce47.com"));

            var request = new RestRequest("api/me", Method.GET, DataFormat.Json);

            try
            {
                var response = await client.ExecuteGetAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Forum response {response.Content}");
                    return JsonConvert.DeserializeObject<ForumUser>(response.Content);
                    
                }
                _logger.LogError($"Something went wrong! Forum answered api/me request with status code:{response.StatusCode} and content:{response.Content}\nRequest Info BaseUrl:{client.BaseUrl} Cookie:{client.CookieContainer.GetCookies(new Uri("https://api.taskforce47.com")).First().Value}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot get data: {ex.Message}");
                return null;
            }
        }
    }
}
