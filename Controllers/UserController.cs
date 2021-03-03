using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Services.Authentication;
using TF47_Backend.Services.OAuth;

namespace TF47_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UserController(ILogger<UserController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("steamCallback/{id:guid}")]
        public async Task<IActionResult> SteamAccountCallback(Guid id)
        {
            using var scope = _serviceProvider.CreateScope();
            var steamAuthenticationService = scope.ServiceProvider.GetService<ISteamAuthenticationService>();
            var result = await steamAuthenticationService.HandleSteamCallback(HttpContext);
            return Ok("success");
        }

        [HttpGet("linkSteam")]
        public async Task<IActionResult> LinkSteamAccount()
        {
            return await Task.Run(() =>
            {
                using var scope = _serviceProvider.CreateScope();
                var steamAuthenticationService = scope.ServiceProvider.GetService<ISteamAuthenticationService>();


                var userId = Guid.NewGuid();
                var guid = steamAuthenticationService.CreateChallenge(userId);

                var url = $"https://steamcommunity.com/openid/login?" + $"{GetHeaders(guid)}";

                _logger.LogInformation($"Request guid: {guid}");
                _logger.LogInformation($"{url}");
                return Redirect(url);
            });
        }

        /* Needed?
        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            
        }*/

        private string GetHeaders(Guid guid)
        {
            var headers = new Dictionary<string, string>
            {
                {"openid.ns", "http://specs.openid.net/auth/2.0"},
                {"openid.mode", "checkid_setup"},
                {"openid.return_to", $"https://{HttpContext.Request.Host.Value}/api/user/steamCallback/{guid}"},
                {"openid.realm", $"https://{HttpContext.Request.Host.Value}"},
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
