using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WhitelistController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<WhitelistController> _logger;

        public WhitelistController(Tf47DatabaseContext database, ILogger<WhitelistController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet("getAvailableWhitelist")]
        public async Task<IActionResult> GetAvailableWhitelist()
        {
            var whitelist = _database.Tf47ServerWhitelists.Where(x => x.Id > 0);
            return Ok(await whitelist.ToArrayAsync());
        }

        [HttpGet("getWhitelistCurrentSession")]
        public async Task<IActionResult> GetUserWhitelistCurrentSession()
        {

            var forumId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.ForumId);
            if (forumId == null) return StatusCode(500, "forum id is null");

            var playerUid = _database.Tf47GadgetUser.First(x => x.ForumId == uint.Parse(forumId.Value)).PlayerUid;
            if (string.IsNullOrEmpty(playerUid)) return BadRequest("missing playeruid!");

            var serverPlayer = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.PlayerUid == playerUid);
            var result = await GetUserWhitelist(new PlayerIdRequest {PlayerId = serverPlayer.Id});

            return Ok(result);

        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("getWhitelistUser")]
        public async Task<IActionResult> GetUserWhitelist([FromForm] PlayerIdRequest playerIdRequest)
        {
            if (!ModelState.IsValid) return BadRequest("model state is not correct!");

            var allWhitelists = _database.Tf47ServerWhitelists.Where(x => x.Id > 0);
            var demoWhitelists = new List<Whitelist>();
            var player = _database.Tf47ServerPlayers.FirstOrDefault(x => x.Id == playerIdRequest.PlayerId);
            if (player == null) return BadRequest("cannot find player");

            foreach (var whitelist in allWhitelists)
            {
                demoWhitelists.Add(new Whitelist
                {
                    Enabled = false,
                    WhitelistName = whitelist.Description,
                    Id = whitelist.Id
                });
            }

            var result = new UserWhitelist
            {
                PlayerId = player.Id,
                PlayerName = player.PlayerName,
                PlayerUid = player.PlayerUid,
                Whitelists = demoWhitelists
            };

            var playerWhitelist =
                _database.Tf47ServerPlayerWhitelisting.Where(whitelist => whitelist.PlayerId == player.Id);

            foreach (var activeWhitelist in playerWhitelist)
            {
                foreach (var whitelist in result.Whitelists)
                {
                    if (whitelist.Id == activeWhitelist.WhitelistId)
                    {
                        whitelist.Enabled = true;
                        break;
                    }
                }
            }

            return Ok(await playerWhitelist.ToArrayAsync());
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("getWhitelistAllUser")]
        public async Task<IActionResult> GetWhitelistAllUsers()
        { 
            var allWhitelists = _database.Tf47ServerWhitelists.Where(x => x.Id > 0).AsNoTracking();
            var demoWhitelists = new List<Whitelist>();
            foreach (var whitelist in allWhitelists)
            {
                demoWhitelists.Add(new Whitelist
                {
                    Enabled = false,
                    WhitelistName = whitelist.Description,
                    Id = whitelist.Id
                });
            }

            //get all users to init the list with UserWhitelistModel DTO Object
            var allUsers = _database.Tf47ServerPlayers.Where(user => user.Id > 0).AsNoTracking(); //get all users
            var result = new List<UserWhitelist>();
            foreach (var user in allUsers)
            {
                result.Add(new UserWhitelist
                {
                    PlayerId = user.Id,
                    PlayerName = user.PlayerName,
                    PlayerUid = user.PlayerUid,
                    Whitelists = demoWhitelists.ConvertAll(whitelist => new Whitelist
                    {
                        Enabled = whitelist.Enabled,
                        WhitelistName = whitelist.WhitelistName,
                        Id = whitelist.Id
                    })
                });
            }

            var allCurrentWhitelistedUser =
                _database.Tf47ServerPlayerWhitelisting.Where(whitelist => whitelist.WhitelistId > 0);

            //iterate over all current existing whitelists and enable those for the corresponding whitelist object of the player
            foreach (var playerWhitelisted in allCurrentWhitelistedUser)
            {
                var player =
                    result.FirstOrDefault(currentPlayer => currentPlayer.PlayerId == playerWhitelisted.PlayerId);
                if (player == null) continue;
                foreach (var whitelist in player.Whitelists)
                {
                    if (whitelist.Id == playerWhitelisted.Id)
                    {
                        whitelist.Enabled = true;
                        break;
                    }
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut("whitelistUser")]
        public async Task<IActionResult> WhitelistUser([FromBody] PlayerWhitelistRequest[] request)
        {
            if (!ModelState.IsValid) return BadRequest();
            foreach (var playerWhitelistRequest in request)
            {
                if (playerWhitelistRequest.Enabled)
                {
                    var whitelist = await _database.Tf47ServerPlayerWhitelisting.FirstOrDefaultAsync(x =>
                        x.PlayerId == playerWhitelistRequest.PlayerId && x.WhitelistId == playerWhitelistRequest.WhitelistId);
                    if (whitelist == null)
                    {
                        await _database.Tf47ServerPlayerWhitelisting.AddAsync(new Tf47ServerPlayerWhitelisting
                        {
                            PlayerId = playerWhitelistRequest.PlayerId,
                            WhitelistId = playerWhitelistRequest.WhitelistId
                        });
                    }
                    else
                        _logger.LogInformation($"Already whitlelisted {playerWhitelistRequest.PlayerId}");
                }
                else
                {
                    var whitelist = await _database.Tf47ServerPlayerWhitelisting.FirstOrDefaultAsync(x =>
                        x.PlayerId == playerWhitelistRequest.PlayerId && x.WhitelistId == playerWhitelistRequest.WhitelistId);
                    if (whitelist != null)
                    { 
                        _database.Tf47ServerPlayerWhitelisting.Remove(whitelist);
                    }
                    else
                        _logger.LogInformation($"No whitelist found {playerWhitelistRequest.PlayerId}!");    
                }
            }

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while trying to save whitelist changes!\n {ex.Message}");
                return StatusCode(500, "Error while trying to save whitelist changes!");
            }

            return Ok();
        }
    }
}