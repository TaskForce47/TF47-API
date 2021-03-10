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
using Microsoft.Extensions.Logging;
using RestSharp;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Dto;
using TF47_Backend.Dto.RequestModels;
using TF47_Backend.Services.Authentication;
using TF47_Backend.Services.Mail;
using TF47_Backend.Services.OAuth;

namespace TF47_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly DatabaseContext _database;
        private readonly MailService _mailService;
        private readonly ISteamAuthenticationService _steamAuthenticationService;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILogger<AuthenticationController> logger, DatabaseContext database,
           MailService mailService, ISteamAuthenticationService steamAuthenticationService,
           IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _database = database;
            _mailService = mailService;
            _steamAuthenticationService = steamAuthenticationService;
            _authenticationManager = authenticationManager;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                var challenge =
                    _steamAuthenticationService.CreateChallenge(HttpContext, "api/Authentication/login/steam/callback");
                return Redirect(challenge);
            }

            return BadRequest("user is already authenticated");
        }

        [AllowAnonymous]
        [HttpGet("login/steam/callback/{guid}")]
        public async Task<IActionResult> HandleSteamLoginCallback(Guid guid)
        {
            var steamUser = await _steamAuthenticationService.HandleSteamCallbackAsync(HttpContext);
            var claimsIdentity = await _authenticationManager.UpdateUserDataAsync(steamUser.Response.Players.First());
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(4)
                });
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("register/steam")]
        public IActionResult RegisterWithSteam()
        {
            var challenge = _steamAuthenticationService.CreateChallenge(HttpContext, "api/Authentication/register/steam/callback");
            return Redirect(challenge);
        }

        [AllowAnonymous]
        [HttpGet("register/steam/callback/{guid}")]
        public async Task<IActionResult> HandleSteamRegisterCallback(Guid guid)
        {
            var steamUser = await _steamAuthenticationService.HandleSteamCallbackAsync(HttpContext);
            var claimsIdentity = await _authenticationManager.CreateUserAsync(steamUser.Response.Players.First());

            if (claimsIdentity != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(4)
                    });
            }
            else
            {
                return BadRequest("something went wrong");
            }

            return Ok();
        }
    }
}
