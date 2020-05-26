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
        public async Task<IActionResult> GetUserWhitelist([FromBody] PlayerIdRequest playerIdRequest)
        {
            if (!ModelState.IsValid) return BadRequest("model state is not correct!");

            var response = new List<UserWhitelistResponse>();
            var availableWhitelist = await _database.Tf47ServerWhitelists.Where(x => x.Id > 0).Select(x => new Whitelist
            {
                Id = x.Id,
                Enabled = false,
                WhitelistName = x.Description
            }).ToListAsync();

            var player = await _database.Tf47ServerPlayers.Include(x => x.Tf47ServerPlayerWhitelisting).FirstOrDefaultAsync(x => x.Id == playerIdRequest.PlayerId);
            if (player == null) return BadRequest("player id does not exist!");
            var userWhitelistResponse = new UserWhitelistResponse
            {
                Id = player.Id,
                PlayerName = player.PlayerName,
                PlayerUid = player.PlayerUid,
                Whitelists = availableWhitelist.ConvertAll(x => x.Clone()).ToList()
            };
            foreach (var enabledWhitelist in player.Tf47ServerPlayerWhitelisting)
            { 
                userWhitelistResponse.Whitelists.First(x => x.Id == enabledWhitelist.WhitelistId).Enabled = true;
            }

            response.Add(userWhitelistResponse);
            return Ok(response);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("getWhitelistAllUser")]
        public async Task<IActionResult> GetWhitelistAllUsers()
        {
            var response = new List<UserWhitelistResponse>();
            var availableWhitelist = await _database.Tf47ServerWhitelists.Where(x => x.Id > 0).Select(x => new Whitelist
            {
                Id = x.Id,
                Enabled = false,
                WhitelistName = x.Description
            }).ToListAsync();

            var playerList = _database.Tf47ServerPlayers.Include(x => x.Tf47ServerPlayerWhitelisting).Where(x => x.Id > 0);
            foreach (var player in playerList)
            {
                var userWhitelistResponse = new UserWhitelistResponse
                {
                    Id = player.Id,
                    PlayerName = player.PlayerName,
                    PlayerUid = player.PlayerUid,
                    Whitelists = availableWhitelist.ConvertAll(x => x.Clone()).ToList()
                };
                foreach (var enabledWhitelist in player.Tf47ServerPlayerWhitelisting)
                {
                    userWhitelistResponse.Whitelists.First(x => x.Id == enabledWhitelist.WhitelistId).Enabled = true;
                }
                response.Add(userWhitelistResponse);
            }

            return Ok(response);
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