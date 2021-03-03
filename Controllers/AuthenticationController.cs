using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestSharp;
using TF47_Backend.Database;
using TF47_Backend.Dto;
using TF47_Backend.Services.Authentication;

namespace TF47_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserManager _userManager;

        public AuthenticationController(ILogger<AuthenticationController> logger, DatabaseContext database, IUserManager userManager)
        {
            _logger = logger;
            _database = database;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResult = await _userManager.AuthenticateUser(request.Username, request.Password);
            if (loginResult == null) return BadRequest("Username or password incorrect");

            return Ok(await loginResult.GetJwtToken());
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!request.AcceptTermsOfService) return BadRequest("You must accept the Terms of Service to register");

            var result = await _userManager.CreateUser(request.Username, request.Password, request.Email);
            if (result == null) return BadRequest("Either the username or email is already in use");

            return Ok(await result.GetJwtToken());
        }

        [HttpGet("/updateToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var guid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "Guid").Value);
            var token = await _userManager.GetTokenProvider().GenerateToken(guid);
            return Ok(token);
        }

        public class UserRolesResponse
        {
            public Guid UserId { get; set; }
            public uint RoleId { get; set; }
            public string RoleDescription { get; set; }
        }
    }
}
