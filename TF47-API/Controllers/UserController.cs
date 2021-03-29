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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.ResponseModels;
using TF47_API.Services;
using TF47_API.Services.Authentication;
using TF47_API.Services.OAuth;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;
        private readonly IDiscordAuthenticationService _discordAuthenticationService;
        private readonly IConfiguration _configuration;

        public UserController(
            ILogger<UserController> logger, 
            DatabaseContext database,
            IUserProviderService userProviderService,
            IDiscordAuthenticationService discordAuthenticationService, 
            IConfiguration configuration)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
            _discordAuthenticationService = discordAuthenticationService;
            _configuration = configuration;
        }
        
        [HttpGet("")]
        [ProducesResponseType(typeof(UserResponse[]), 200)]
        public async Task<IActionResult> GetDetailUsers()
        {
            var userResponses = await _database.Users
                .AsNoTracking()
                .Include(x => x.WrittenNotes)
                .Include(x => x.WrittenChangelogs)
                .Include(x => x.Groups)
                .ThenInclude(y => y.GroupPermission)
                .ToListAsync();

            return Ok(userResponses.AsEnumerable().ToUserResponseIEnumerable(true));
        }
        
        
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> GetUserDetail()
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            if (user == null)
                return BadRequest("You must be logged in to query this endpoint");

            var userDetail = await _database.Users
                .AsNoTracking()
                .Include(x => x.ApiKeys)
                .Include(x => x.WrittenNotes)
                .Include(x => x.WrittenChangelogs)
                .Include(x => x.Groups)
                .ThenInclude(y => y.GroupPermission)
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            return Ok(userDetail.ToUserResponse());
        }
        
        [HttpGet("{userid:guid}/")]
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            //var userGuid = Guid.Parse(userId);
            var userDetails = await _database.Users
                .Include(x => x.Groups)
                .ThenInclude(z => z.GroupPermission)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userDetails == null) return BadRequest("Requested user details not found");
            
            return Ok(userDetails.ToUserResponse());
        }

        [HttpGet("link/Discord")]
        public IActionResult LinkDiscord()
        {
            var challenge = _discordAuthenticationService.CreateChallenge(HttpContext, "api/User/link/callback/discord");
            return Redirect(challenge);
        }

        [HttpGet("link/callback/Discord")]
        public async Task<IActionResult> HandleDiscordCallback()
        {
            var result = (DiscordUserResponse)await _discordAuthenticationService.HandleCallbackAsync(HttpContext);
            if (result == null) return Redirect(_configuration["Redirections:LinkFailed"]);

            var user = await _userProviderService.GetDatabaseUser(HttpContext);
            user.DiscordId = result.Id;
            user.Email = result.Email;

            await _database.SaveChangesAsync();
            return Redirect(_configuration["Redirections:LinkSuccessful"]);
        }

    }
}
