using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.DTO;
using TF47_Api.Models;
using TF47_Api.Services;

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<UserController> _logger;
        private readonly AuthenticationProviderService _authenticationProviderService;
        private readonly GadgetUserProviderService _gadgetUserProviderService;

        public UserController(Tf47DatabaseContext database, ILogger<UserController> logger, AuthenticationProviderService authenticationProviderService, GadgetUserProviderService gadgetUserProviderService)
        {
            _database = database;
            _logger = logger;
            _authenticationProviderService = authenticationProviderService;
            _gadgetUserProviderService = gadgetUserProviderService;
        }

        [AllowAnonymous]
        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var cookie = HttpContext.Request.Cookies["express.sid"];
            if (string.IsNullOrEmpty(cookie)) return BadRequest("no cookie found");
            var user = await _authenticationProviderService.AuthenticateUserAsync(cookie);
            if (user == null) return StatusCode(403, "could not authorize");

            return Ok();
        }

        [HttpGet("getRoles")]
        public IActionResult GetRoles()
        {
            var user = HttpContext.User;
            var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(claim => claim.Value).ToArray();
            return Ok(roles);
        }

        [HttpGet("getSquads")]
        public async Task<IActionResult> GetSquads()
        {
            var gadgetUser =  await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            var squads = _database.Tf47GadgetSquadUser
                .Include(x => x.User)
                .Include(x => x.Squad)
                .Where(x => x.UserId == gadgetUser.Id)
                .Select(x => new
                {
                    SquadId = x.Squad.Id,
                    x.Squad.SquadNick,
                    x.Squad.SquadTitle,
                    x.Squad.SquadEmail,
                    x.Squad.SquadWeb,
                    x.Squad.SquadHasPicture,
                    Remark = x.UserSquadRemark,
                    Email = x.UserSquadEmail
                });
            return Ok(squads);
        }

        [HttpGet("getUserDetails")]
        public async Task<IActionResult> GetPlayerDetails()
        {
            var user = HttpContext.User;
            var forumId = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ForumId)?.Value;
            var databaseUser =
                await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.ForumId == uint.Parse(forumId));
            if (databaseUser == null)
            {
                return StatusCode(500, "Something went wrong. Cannot find user in database!");
            }

            var userDetails = new UserDetails
            {
                ProfileName = user.Identity.Name,
                Avatar = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ProfilePicture)?.Value,
                ForumId = uint.Parse(forumId),
                PlayerUid = databaseUser.PlayerUid,
                Roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(claim => claim.Value).ToArray()
            };
            _logger.LogInformation($"{userDetails.ForumId} {userDetails.ProfileName}");
            return Ok(userDetails);
        }

        [HttpPut("setUserPlayerUid")]
        public async Task<IActionResult> SetPlayerUid([FromBody] PlayerUidRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("bad request");

            var forumUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);

            var player = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
            if (player == null)
            {
                return BadRequest("PlayerUid never seen on the server!");
            }

            _logger.LogInformation(
                $"Setting player uid {request.PlayerUid} for gadget user {forumUser.ForumName}, server name {player.PlayerName}!");
            forumUser.PlayerUid = request.PlayerUid;
            await _database.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeletePlayerUidCurrentSession")]
        public async Task<IActionResult> DeletePlayerUidCurrentSession()
        {
            var forumUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);

            forumUser.PlayerUid = null;
            await _database.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Moderator, Administrator")]
        [HttpDelete("DeletePlayerUid")]
        public async Task<IActionResult> DeleteGadgetPlayerUid([FromBody] GadgetIdRequest request)
        {
            var user = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (user == null)
            {
                return Ok("no user found");
            }

            user.PlayerUid = null;
            await _database.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("getWhitelist")]
        public async Task<IActionResult> GetWhitelist()
        {
            var gadgetUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            var whitelists = _database.Tf47ServerPlayerWhitelisting
                .Include(x => x.Player)
                .Include(x => x.Whitelist)
                .Where(x => x.Player.PlayerUid == gadgetUser.PlayerUid)
                .Select(x => new
                {
                    WhitelistingId = x.Id,
                    x.PlayerId,
                    x.Player.PlayerName,
                    x.Whitelist.Id,
                    x.Whitelist.Description
                });

            return Ok(whitelists);
        }
    }
}