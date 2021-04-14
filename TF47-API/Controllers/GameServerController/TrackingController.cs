using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer.AAR;
using TF47_API.Dto.RequestModels;

namespace TF47_API.Controllers.GameServerController
{
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

        [Authorize]
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

        [Authorize]
        [HttpPost("{sessionId:int}/batch")]
        public async Task<IActionResult> CreatePlayItemsBatch(long sessionId,
            [FromBody] CreateReplayItemRequest[] request)
        {
            var session = await _database.Sessions.FindAsync(sessionId);

            var replayItems = new List<ReplayItem>();
            foreach (var createReplayItemRequest in request)
            {
                var replayItem = new ReplayItem
                {
                    Data = createReplayItemRequest.Data,
                    GameTime = createReplayItemRequest.GameTime,
                    GameTickTime = createReplayItemRequest.GameTickTime,
                    TrackingId = createReplayItemRequest.TrackingId,
                    Type = createReplayItemRequest.Type,
                    Session = session
                };
                replayItems.Add(replayItem);
            }
            
            try
            {
                await _database.ReplayItems.AddRangeAsync(replayItems);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert replay item: {message}", ex.Message);
                return Problem("Failed to create replay item", null, 500, "Failed to create replay item");
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("{sessionId:int}")]
        public async Task<IActionResult> GetReplayItems(long sessionId)
        {
            var result = await _database.ReplayItems
                .Where(x => x.SessionId == sessionId)
                .ToListAsync();

            return Ok(result);
        }
    }
}