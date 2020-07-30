using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.CustomStatusCodes;
using TF47_Api.Database;
using TF47_Api.DTO;
using TF47_Api.Models;
using TF47_Api.Services;

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WhitelistController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<WhitelistController> _logger;
        private readonly GadgetUserProviderService _gadgetUserProviderService;

        public WhitelistController(Tf47DatabaseContext database, ILogger<WhitelistController> logger, GadgetUserProviderService gadgetUserProviderService)
        {
            _database = database;
            _logger = logger;
            _gadgetUserProviderService = gadgetUserProviderService;
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
            var result = await GetUserWhitelist(serverPlayer.Id);

            return Ok(result);

        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("{id}/getWhitelist")]
        public async Task<IActionResult> GetUserWhitelist(uint id)
        {
            var response = new List<UserWhitelistResponse>();
            var availableWhitelist = await _database.Tf47ServerWhitelists
                .Where(x => x.Id > 0)
                .Select(x => new Whitelist
            {
                Id = x.Id,
                Enabled = false,
                WhitelistName = x.Description
            }).ToListAsync();

            var player = await _database.Tf47ServerPlayers
                .Include(x => x.Tf47ServerPlayerWhitelisting)
                .FirstOrDefaultAsync(x => x.Id == id);
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

            var requestUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);

            var availableWhitelist = _database.Tf47ServerWhitelists.Where(x => x.Id > 0).ToArray();
            var enableRequests = request.Where(x => x.Enabled);
            var disableRequests = request.Where(x => x.Enabled == false);

            foreach (var enableRequest in enableRequests)
            {
                var user = await _database.Tf47ServerPlayers
                    .Include(x => x.Tf47ServerPlayerWhitelisting)
                    .FirstOrDefaultAsync(x => x.Id == enableRequest.PlayerId);

                var whitelist = user.Tf47ServerPlayerWhitelisting.FirstOrDefault(
                    x => x.WhitelistId == enableRequest.WhitelistId && x.PlayerId == enableRequest.PlayerId);
                if(whitelist == null)
                {
                    await _database.Tf47ServerPlayerWhitelisting.AddAsync(new Tf47ServerPlayerWhitelisting
                    {
                        PlayerId = enableRequest.PlayerId,
                        WhitelistId = enableRequest.WhitelistId
                    });
                    var action =
                        $"{user.PlayerName} has been whitelisted for {availableWhitelist.First(x => x.Id == enableRequest.WhitelistId).Description} by {requestUser.ForumName}";
                    await _database.Tf47GadgetActionLog.AddAsync(new Tf47GadgetActionLog
                    {
                        Action = action,
                        ActionPerformed = DateTime.Now,
                        UserId = requestUser.Id
                    });
                    await _database.Tf47GadgetUserNotes.AddAsync(new Tf47GadgetUserNotes
                    {
                        AuthorId = requestUser.Id,
                        PlayerId = user.Id,
                        PlayerNote = action,
                        TimeWritten = DateTime.Now,
                        Type = "Whitelist added"
                    });
                }
            }

            foreach (var disableRequest in disableRequests)
            {
                var user = await _database.Tf47ServerPlayers
                    .Include(x => x.Tf47ServerPlayerWhitelisting)
                    .FirstOrDefaultAsync(x => x.Id == disableRequest.PlayerId);

                var whitelist = user.Tf47ServerPlayerWhitelisting.FirstOrDefault(
                    x => x.WhitelistId == disableRequest.WhitelistId);

                if(whitelist != null)
                {
                    _database.Tf47ServerPlayerWhitelisting.Remove(whitelist);

                    var action =
                        $"{user.PlayerName} has been removed from the {availableWhitelist.First(x => x.Id == disableRequest.WhitelistId).Description} whitelist by {requestUser.ForumName}";
                    await _database.Tf47GadgetActionLog.AddAsync(new Tf47GadgetActionLog
                    {
                        Action = action,
                        ActionPerformed = DateTime.Now,
                        UserId = requestUser.Id
                    });
                    await _database.Tf47GadgetUserNotes.AddAsync(new Tf47GadgetUserNotes
                    {
                        AuthorId = requestUser.Id,
                        PlayerId = user.Id,
                        PlayerNote = action,
                        TimeWritten = DateTime.Now,
                        Type = "Whitelist removed"
                    });
                }
            }

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var error = $"Something went wrong while trying to save whitelist updates: {ex.Message}";
                _logger.LogError(error);
                return new ServerError(error);
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("GetUserByWhitelist/{page}")]
        public async Task<IActionResult> GetUsersByWhitelist(
            int page, 
            [FromQuery(Name = "WhitelistId")][Required, Range(1, Int32.MaxValue)] int whitelistId = -1, 
            [FromQuery(Name = "rows")]int rows = 20)
        {
            if (page < 1) page = 1;
            page--;

            var result = await Task.Run(() =>
            {
                var totalUsersWithWhitelist =
                    _database.Tf47ServerPlayerWhitelisting
                        .Count(x => x.WhitelistId == whitelistId);
                var usersByWhitelist =_database.Tf47ServerPlayerWhitelisting
                    .Include(x => x.Player)
                    .Include(x => x.Whitelist)
                    .Where(x => x.WhitelistId == whitelistId)
                    .OrderByDescending(x => x.Id)
                    .Skip(rows * page)
                    .Take(rows)
                    .Select(x => new
                    {
                        x.Id,
                        x.PlayerId,
                        x.Player.PlayerName,
                        x.WhitelistId,
                        x.Whitelist.Description
                    });
                return new
                {
                    TotalUsersWithWhitelist = totalUsersWithWhitelist,
                    UsersByWhitelist = usersByWhitelist
                };
            });


            return Ok(result);
        }
    }
}