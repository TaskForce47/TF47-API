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

        public async Task<IActionResult> GetPlayerDetails()
        {
            return NotFound("not implemented yet!");
        }

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
    }
}
