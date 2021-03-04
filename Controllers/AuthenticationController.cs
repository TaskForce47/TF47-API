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

namespace TF47_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserManager _userManager;
        private readonly MailService _mailService;

        public AuthenticationController(ILogger<AuthenticationController> logger, DatabaseContext database,
            IUserManager userManager, MailService mailService)
        {
            _logger = logger;
            _database = database;
            _userManager = userManager;
            _mailService = mailService;
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

        [HttpPost("/resetPasswordRequest")]
        public async Task<IActionResult> ResetPasswordRequest()
        {
            var guid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "Guid").Value);
            var user = await _database.Users.FirstOrDefaultAsync(x => x.UserId == guid);
    
            var passwordReset = await _database.PasswordResets
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.User.UserId == guid && (DateTime.UtcNow - x.TimePasswordResetGenerated) < TimeSpan.FromHours(24));

            using var cryptoProvider = new SHA512CryptoServiceProvider();
            var resetTokenBytes = Encoding.UTF8.GetBytes(user.Password).Concat(
                                  Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("F"))).ToArray();
            var resetTokenHash = cryptoProvider.ComputeHash(resetTokenBytes);


            var stringBuilder = new StringBuilder();
            foreach (var b in resetTokenBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            var resetToken = stringBuilder.ToString();

            if (passwordReset == null)
            {
                passwordReset = new PasswordReset
                {
                    User = user,
                    ResetToken = resetToken,
                    TimePasswordResetGenerated = DateTime.UtcNow
                };
                await _database.AddAsync(passwordReset);
            }
            else
            {
                passwordReset.ResetToken = resetToken;
                passwordReset.TimePasswordResetGenerated = DateTime.UtcNow;
                
            }
            await _database.SaveChangesAsync();

            var mailBody = "You are receiving this mail because you tried to reset your account at taskforce47.\n" +
                           "If you did not request a password reset you can ignore this mail.\n" +
                           $"Otherwise you can press this link: {resetToken}";

            await _mailService.SendMailAsync(user.Mail, user.Username, "Reset password request",
                mailBody, CancellationToken.None);

            _logger.LogInformation($"Send password reset mail to {user.Username} {user.Mail}");

            return Ok();
        }

        [HttpPost("/updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            var guid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "Guid").Value);
            var user = await _database.Users.FirstOrDefaultAsync(x => x.UserId == guid);

            var isAuthenticatedUser = await _userManager.AuthenticateUser(user.Username, updatePasswordRequest.OldPassword);
            if (isAuthenticatedUser == null) return BadRequest("password wrong");

            await _userManager.UpdatePasswordAsync(user.UserId, updatePasswordRequest.NewPassword);
            return Ok();
        }

        [HttpPost("/updatePasswordToken")]
        public async Task<IActionResult> UpdatePasswordByToken([FromBody] UpdatePasswordToken updatePasswordToken)
        {
            var guid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "Guid").Value);
            var user = await _database.Users
                .Include(x => x.UserPasswordResets)
                .FirstOrDefaultAsync(x => x.UserId == guid);

            var passwordReset = user.UserPasswordResets.FirstOrDefault(x =>
                x.ResetToken == updatePasswordToken.Token &&
                DateTime.UtcNow - x.TimePasswordResetGenerated < TimeSpan.FromHours(24));

            if (passwordReset == null)
                return BadRequest("Token does not exist");

            await _userManager.UpdatePasswordAsync(user.UserId, updatePasswordToken.NewPassword);

            return Ok();
        }
    }
}
