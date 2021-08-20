using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto;
using TF47_API.Dto.RequestModels;
using TF47_API.Services.Authentication;
using TF47_API.Services.Mail;
using TF47_API.Services.OAuth;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ISteamAuthenticationService _steamAuthenticationService;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            ISteamAuthenticationService steamAuthenticationService, 
            IAuthenticationManager authenticationManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _steamAuthenticationService = steamAuthenticationService;
            _authenticationManager = authenticationManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("login/steam")]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                var challenge =
                    _steamAuthenticationService.CreateChallenge(HttpContext, "api/Authentication/login/callback/steam");
                return Redirect(challenge);
            }

            return BadRequest("user is already authenticated");
        }

        [AllowAnonymous]
        [HttpGet("login/callback/steam")]
        public async Task<IActionResult> HandleSteamLoginCallback()
        {
            var steamUser = (SteamUserResponse)await _steamAuthenticationService.HandleCallbackAsync(HttpContext);

            if (steamUser.Response.Players.FirstOrDefault() == null)
            {
                _logger.LogError($"Steam did not return a user");
                return Redirect(_configuration["Redirections:AuthenticationFailed"]);
            }

            var claimsIdentity = await _authenticationManager.UpdateOrCreateUserAsync(steamUser.Response.Players.First());
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(4)
                });

            return Redirect(_configuration["Redirections:AuthenticationSuccessful"]);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
