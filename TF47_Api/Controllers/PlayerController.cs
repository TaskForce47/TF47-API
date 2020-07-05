#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using TF47_Api.CustomStatusCodes;
using TF47_Api.Database;

namespace TF47_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerNotesController> _logger;
        private readonly Tf47DatabaseContext _database;

        public PlayerController(ILogger<PlayerNotesController> logger, Tf47DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("GetAllPlayers")]
        public async Task<IActionResult> GetAllPlayers()
        {
            return await Task.Run(() =>
            {
                var temp = _database.Tf47ServerPlayers.Where(x => x.Id > 0).Select(x => new ServerPlayer
                {
                    Id = x.Id,
                    Name = x.PlayerName,
                    Uid = x.PlayerUid
                });
                return Ok(temp);
            });
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetPlayerDetails(uint id)
        {
            var serverPlayer = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.Id == id);
            if (serverPlayer == null) return BadRequest("user not found!");

            var gadgetUser =
                await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.PlayerUid == serverPlayer.PlayerUid);

            var response = new UserDetailResponse
            {
                Player = new ServerPlayer
                {
                    Id = serverPlayer.Id,
                    Name = serverPlayer.PlayerName,
                    Uid = serverPlayer.PlayerUid
                },
                User = new GadgetUser
                {
                    Id = gadgetUser.Id,
                    AvatarUrl = gadgetUser.ForumAvatarPath,
                    ForumName = gadgetUser.ForumName,
                    Roles = GetRolesFromGadgetUser(gadgetUser)
                }
            };
            return Ok(response);
        }

        [HttpGet("GetPlayerDetailsLoggedIn")]
        public async Task<IActionResult> GetPlayerDetailsLoggedIn()
        {
            return NotFound("not implemented yet!");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("DeletePlayer")]
        public async Task<IActionResult> DeletePlayer([FromBody] ServerPlayerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (user == null) return BadRequest("user does not exist");
            try
            {
                _database.Tf47ServerPlayers.Remove(user);
                await _database.SaveChangesAsync();
                _logger.LogError($"Removed player {user.PlayerName} from database");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot remove public player {user.PlayerName} from database! error: {ex.Message}");
                return new ServerError("could not remove player from the database");
            }

            return Ok();
        }

        public class ServerPlayer
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Uid { get; set; }
        }

        public class ServerPlayerRequest
        {
            public uint Id { get; set; }
        }

        public class GadgetUser
        {
            public uint? Id { get; set; }
            public string? ForumName { get; set; }
            public string? AvatarUrl { get; set; }

            public IEnumerable<string>? Roles { get; set; } 
        }

        public class UserDetailResponse
        {
            public ServerPlayer Player { get; set; }
            public GadgetUser User { get; set; }
        }

        private IEnumerable<string> GetRolesFromGadgetUser(Tf47GadgetUser user)
        {
            var roles = new List<string>();
            if (user.ForumIsAdmin != null && user.ForumIsAdmin.Value)
                roles.Add("Admin");
            if(user.ForumIsModerator != null && user.ForumIsModerator.Value)
                roles.Add("Moderator");
            if(user.ForumIsTf != null && user.ForumIsTf.Value)
                roles.Add("TF47");
            if(user.ForumIsSponsor != null && user.ForumIsSponsor.Value)
                roles.Add("Sponsor");
            return roles;
        }
    }
}