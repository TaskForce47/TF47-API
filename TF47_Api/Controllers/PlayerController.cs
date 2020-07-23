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
using TF47_Api.Services;

namespace TF47_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerNotesController> _logger;
        private readonly Tf47DatabaseContext _database;
        private readonly GadgetUserProviderService _gadgetUserProviderService;

        public PlayerController(ILogger<PlayerNotesController> logger, Tf47DatabaseContext database, GadgetUserProviderService gadgetUserProviderService)
        {
            _logger = logger;
            _database = database;
            _gadgetUserProviderService = gadgetUserProviderService;
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
                Uid = serverPlayer.PlayerUid,
                IsBanned = serverPlayer.IsBanned,
                BannedUntil = serverPlayer.BannedUntil
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

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanUser(uint id, [FromBody] BanPlayerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var gadgetUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            var player = await _database.Tf47ServerPlayers
                .FirstOrDefaultAsync(x => x.Id == id);
            if (player == null) return BadRequest("Player id doesn't exist!");
            player.IsBanned = true;
            player.BannedUntil = request.BannedUntil;
            try
            {
                _database.Update(player);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save ban for user {player.PlayerName}\nReason: {ex.Message}");
                return new ServerError($"Error saving ban for user {player.PlayerName}");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/pardon")]
        public async Task<IActionResult> PardonUser(uint id, [FromBody] BanPlayerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var gadgetUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            var player = await _database.Tf47ServerPlayers
                .FirstOrDefaultAsync(x => x.Id == id);
            if (player == null) return BadRequest("Player id doesn't exist!");
            player.IsBanned = false;
            player.BannedUntil = null;
            try
            {
                _database.Update(player);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save delete ban for user {player.PlayerName}\nReason: {ex.Message}");
                return new ServerError($"Error saving deletion of ban for user {player.PlayerName}");
            }
            return Ok();
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> Stats(uint id)
        {
            var player = await _database.Tf47ServerPlayers
                .Include(x => x.Tf47ServerPlayerStats)
                .Include(x => x.Tf47ServerPlayerStatsCreatedOnce)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (player == null) return BadRequest("player does not exist!");


            return Ok(new PlayerStats
            {
                Id = player.Id,
                PlayerName = player.PlayerName,
                PlayerUid = player.PlayerUid,
                IsBanned = player.IsBanned,
                BannedUntil = player.BannedUntil,
                PlayerNameFirstConnect = player.Tf47ServerPlayerStatsCreatedOnce.PlayerNameConnected,
                PlayerFirstTimeSeen = player.Tf47ServerPlayerStatsCreatedOnce.FirstConnectionTime,
                LastTimeSeen = player.Tf47ServerPlayerStats.LastTimeSeen,
                TimePlayedTotal = player.Tf47ServerPlayerStats.TimePlayedBase +
                                  player.Tf47ServerPlayerStats.TimePlayedInf +
                                  player.Tf47ServerPlayerStats.TimePlayedObjective +
                                  player.Tf47ServerPlayerStats.TimePlayedVehHelo +
                                  player.Tf47ServerPlayerStats.TimePlayedVehPlane +
                                  player.Tf47ServerPlayerStats.TimePlayedVehSmall +
                                  player.Tf47ServerPlayerStats.TimePlayedVehTracked,
                TimePlayedBase = player.Tf47ServerPlayerStats.TimePlayedBase,
                TimePlayedObjective = player.Tf47ServerPlayerStats.TimePlayedObjective,
                TimePlayedInfantry = player.Tf47ServerPlayerStats.TimePlayedInf,
                TimePlayedVehicleSmall = player.Tf47ServerPlayerStats.TimePlayedVehSmall,
                TimePlayedVehicleTracked = player.Tf47ServerPlayerStats.TimePlayedVehTracked,
                TimePlayedVehicleHelicopter = player.Tf47ServerPlayerStats.TimePlayedVehHelo,
                TimePlayedVehiclePlane = player.Tf47ServerPlayerStats.TimePlayedVehPlane,
                KillsInfantry = player.Tf47ServerPlayerStats.KillsInf,
                KillsVehicleSmall = player.Tf47ServerPlayerStats.TimePlayedVehSmall,
                KillsVehicleTracked = player.Tf47ServerPlayerStats.KillsVehTracked,
                KillsVehicleHelicopter = player.Tf47ServerPlayerStats.KillsVehHelo,
                KillsVehiclePlane = player.Tf47ServerPlayerStats.KillsVehPlane,
                DeathsInfantry = player.Tf47ServerPlayerStats.DeathsInf,
                DeathsVehicleSmall = player.Tf47ServerPlayerStats.DeathsVehSmall,
                DeathsVehicleTracked = player.Tf47ServerPlayerStats.DeathsVehTracked,
                DeathsVehicleHelicopter = player.Tf47ServerPlayerStats.DeathsVehHelo,
                DeathsVehiclePlane = player.Tf47ServerPlayerStats.DeathsVehPlane
            });
        }

        public class PlayerStats
        {
            public uint Id { get; set; }
            public string PlayerName { get; set; }
            public string PlayerUid { get; set; }
            public string PlayerNameFirstConnect { get; set; }
            public DateTime? PlayerFirstTimeSeen { get; set; }
            public DateTime? LastTimeSeen { get; set; }
            public uint? TimePlayedTotal { get; set; }
            public uint? TimePlayedBase { get; set; }
            public uint? TimePlayedObjective { get; set; }
            public uint? TimePlayedInfantry { get; set; }
            public uint? TimePlayedVehicleSmall { get; set; }
            public uint? TimePlayedVehicleTracked { get; set; }
            public uint? TimePlayedVehicleHelicopter { get; set; }
            public uint? TimePlayedVehiclePlane { get; set; }
            public uint? KillsInfantry { get; set; }
            public uint? KillsVehicleSmall { get; set; } 
            public uint? KillsVehicleTracked { get; set; }
            public uint? KillsVehicleHelicopter { get; set; }
            public uint? KillsVehiclePlane { get; set; }
            public uint? DeathsInfantry { get; set; }
            public uint? DeathsVehicleSmall { get; set; }
            public uint? DeathsVehicleTracked { get; set; }
            public uint? DeathsVehicleHelicopter { get; set; }
            public uint? DeathsVehiclePlane { get; set; }
            public bool IsBanned { get; set; }
            public DateTime? BannedUntil { get; set; }
        }

        public class ServerPlayer
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public string Uid { get; set; }
            public bool IsBanned { get; set; }
            public DateTime? BannedUntil { get; set; }
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
            if (user.ForumIsAdmin)
                roles.Add("Admin");
            if(user.ForumIsModerator)
                roles.Add("Moderator");
            if(user.ForumIsTf != null && user.ForumIsTf.Value)
                roles.Add("TF47");
            if(user.ForumIsSponsor != null && user.ForumIsSponsor.Value)
                roles.Add("Sponsor");
            return roles;
        }

        public class BanPlayerRequest
        {
            public DateTime BannedUntil { get; set; }
        }
    }
}