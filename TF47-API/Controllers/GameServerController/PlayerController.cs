using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;

namespace TF47_API.Controllers.GameServerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly DatabaseContext _database;

        public PlayerController(
            ILogger<PlayerController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("{playerUid}")]
        public async Task<IActionResult> GetPlayer(string playerUid)
        {
            var player = await _database.Players
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PlayerUid == playerUid);

            if (player == null) return BadRequest("PlayerUid provided does not exist");

            return Ok(player.ToPlayerResponse());
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _database.Players
                .AsNoTracking()
                .ToListAsync();
            return Ok(players);
        }

        [HttpPatch("{playerUid}")]
        public async Task<IActionResult> UpdatePlayerName(string playerUid, [FromBody] UpdatePlayerNameRequest request)
        {
            var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == playerUid);
            if (player == null) return BadRequest("PlayerUid provided does not exist");

            player.PlayerName = request.PlayerName;
            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update playername for {uid}: {message}", playerUid, ex.Message);
                return Problem("Failed to update player name. Maybe the player got deleted while trying to update.",
                    null, 500, "Failed to update player name");
            }

            return Ok(player.ToPlayerResponse());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);

            if (player != null) return BadRequest("PlayerUid is already tracked in the database");

            player = new Player
            {
                NumberConnections = 1,
                PlayerUid = request.PlayerUid,
                PlayerName = request.PlayerName,
                TimeFirstVisit = DateTime.Now,
                TimeLastVisit = DateTime.Now
            };

            try
            {
                await _database.Players.AddAsync(player);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new player {uid}: {message}", request.PlayerUid, ex.Message);
                return Problem(
                    "Failed to create new player. Most likely it has been already added while this function was still working.",
                    null, 500, "Failed to create new player");
            }

            return CreatedAtAction(nameof(GetPlayer), new {PlayerUid = player.PlayerUid}, player.ToPlayerResponse());
        }
    }
}