using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer.AAR;

namespace TF47_API.Controllers.GameServerController
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : Controller
    {
        private readonly ILogger<TrackingController> _logger;
        private readonly DatabaseContext _database;

        public TrackingController(
            ILogger<TrackingController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpPost("{sessionId:int}")]
        public async Task<IActionResult> CreateReplayItem(long sessionId, [FromBody] CreateReplayItemRequest request)
        {
            var session = await _database.Sessions.FindAsync(sessionId);

            var replayItem = new ReplayItem
            {
                Data = request.Data,
                GameTime = request.GameTime,
                GameTickTime = request.GameTickTime,
                TrackingId = request.TrackingId,
                Type = request.Type,
                Session = session
            };

            try
            {
                await _database.ReplayItems.AddAsync(replayItem);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert replay item: {message}", ex.Message);
                return Problem("Failed to create replay item", null, 500, "Failed to create replay item");
            }

            return Ok();
        }

        public record CreateReplayItemRequest(long TrackingId, string Type, string Data, float GameTickTime,
            string GameTime);
    }
}