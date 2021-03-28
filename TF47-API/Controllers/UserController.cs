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

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> GetUserDetail()
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            if (user == null)
                return BadRequest("You must be logged in to query this endpoint");

            var userDetail = await _database.Users
                .Include(x => x.ApiKeys)
                .Include(x => x.WrittenNotes)
                .Include(x => x.WrittenChangelogs)
                .Include(x => x.Groups)
                .ThenInclude(y => y.GroupPermission)
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            var userResponse = new UserResponse(userDetail.UserId, userDetail.Banned, userDetail.Email,
                userDetail.Username, userDetail.AllowEmails, userDetail.CountryCode,
                userDetail.DiscordId, userDetail.ProfilePicture, userDetail.ProfileUrl, userDetail.SteamId,
                userDetail.FirstTimeSeen, userDetail.LastTimeSeen,
                userDetail.WrittenNotes
                    .Select(y => new NotesResponse(y.NoteId, y.Player.PlayerUid, y.Player.PlayerName, y.Type,
                        y.Text, y.Writer.UserId, y.Writer.Username, y.TimeCreated, y.TimeLastUpdate)),
                userDetail.WrittenChangelogs
                    .Select(y => new ChangelogResponse(y.ChangelogId, y.Title, y.Tags, y.Text, y.TimeReleased)),
                userDetail.Groups
                    .Select(y => new GroupResponse(y.GroupId, y.Name, y.Description, y.TextColor,
                        y.BackgroundColor, y.IsVisible, new GroupPermissionsResponse(y.GroupPermission.GroupId,
                            y.GroupPermission.PermissionsDiscord, y.GroupPermission.PermissionsTeamspeak,
                            y.GroupPermission.PermissionsGadget))),
                userDetail.ApiKeys.Select(y => new ApiKeysResponse(y.ApiKeyId, y.OwnerId, userDetail.Username, y
                    .ApiKeyValue, y.TimeCreated, y.ValidUntil)));
            
            return Ok(userResponse);
        }
        
        [HttpGet("{userid:guid}/get")]
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            //var userGuid = Guid.Parse(userId);
            var userDetails = await _database.Users
                .Include(x => x.Groups)
                .ThenInclude(z => z.GroupPermission)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return Ok(userDetails);
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
