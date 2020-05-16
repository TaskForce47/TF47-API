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

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<UserController> _logger;

        public UserController(Tf47DatabaseContext database, ILogger<UserController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet("getRoles")]
        public IActionResult GetRoles()
        {
            var user = HttpContext.User;
            var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(claim => claim.Value).ToArray();
            return Ok(roles);
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

            var user = HttpContext.User;
            var forumId = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ForumId)?.Value;
            var databaseUser =
                await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.ForumId == uint.Parse(forumId));
            if (databaseUser == null)
            {
                return StatusCode(500, "Something went wrong. Cannot find user in database!");
            }

            var player = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
            if (player == null)
            {
                return BadRequest("PlayerUid never seen on the server!");
            }

            _logger.LogInformation(
                $"Setting player uid {request.PlayerUid} for gadget user {databaseUser.ForumName}, server name {player.PlayerName}!");
            databaseUser.PlayerUid = request.PlayerUid;
            await _database.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeletePlayerUidCurrentSession")]
        public async Task<IActionResult> DeletePlayerUidCurrentSession()
        {
            var user = HttpContext.User;
            var forumId = user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ForumId)?.Value;
            var databaseUser =
                await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.ForumId == uint.Parse(forumId));
            if (databaseUser == null)
            {
                return StatusCode(500, "Something went wrong. Cannot find user in database!");
            }

            databaseUser.PlayerUid = null;
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
    }
}