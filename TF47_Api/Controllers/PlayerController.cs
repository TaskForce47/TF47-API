using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            var response = new ServerPlayer
            {
                Id = serverPlayer.Id,
                Name = serverPlayer.PlayerName,
                Uid = serverPlayer.PlayerUid
            };
            if (gadgetUser != null)
            {
                response.GadgetUser = new GadgetUser
                {
                    Id = gadgetUser.Id,
                    Roles = GetRolesFromGadgetUser(gadgetUser),
                    AvatarUrl = gadgetUser.ForumAvatarPath,
                    ForumName = gadgetUser.ForumName
                };
            }
            return Ok(response);
        }

        [HttpGet("{id}/getNotes")]
        public async Task<IActionResult> GetPlayerNotes(uint id)
        {
            if (!ModelState.IsValid) return BadRequest();


            var temp = await _database.Tf47ServerPlayers
                .Include(x => x.Tf47GadgetUserNotes)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (temp == null) return BadRequest("player does not exist!");

            var result = new PlayerNoteResponse
            {
                PlayerId = temp.Id,
                PlayerName = temp.PlayerName,
                Notes = temp.Tf47GadgetUserNotes.Select(x => new PlayerNote
                {
                    NodeId = x.Id,
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.ForumName,
                    TimeWritten = x.TimeWritten,
                    Note = x.PlayerNote,
                    Type = x.Type,
                    IsModified = x.IsModified,
                    LastTimeModified = x.LastTimeModified
                }).ToList()
            };
            return Ok(result);
        }

        [HttpGet("GetPlayerDetailsLoggedIn")]
        public async Task<IActionResult> GetPlayerDetailsLoggedIn()
        {
            return NotFound("not implemented yet!");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeletePlayer(uint id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _database.Tf47ServerPlayers.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest("user not found!");
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
            public GadgetUser GadgetUser { get; set; }
        }


        public class GadgetUser
        {
            public uint Id { get; set; }
            public string ForumName { get; set; }
            public string AvatarUrl { get; set; }

            public IEnumerable<string> Roles { get; set; } 
        }

        public class PlayerNoteResponse
        {
            public uint PlayerId { get; set; }
            public string PlayerName { get; set; }

            public List<PlayerNote> Notes { get; set; }
        }

        public class PlayerNote
        {
            public uint NodeId { get; set; }
            public uint AuthorId { get; set; }
            public string AuthorName { get; set; }
            public DateTime TimeWritten { get; set; }
            public string Note { get; set; }
            public string Type { get; set; }
            public bool IsModified { get; set; }
            public DateTime? LastTimeModified { get; set; }
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