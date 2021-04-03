using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.Response;

namespace TF47_API.Controllers.ChangelogControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly ILogger<SessionController> _logger;
        private readonly DatabaseContext _database;

        public SessionController(
            ILogger<SessionController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SessionResponse), 201)]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
        {
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == request.MissionId);
            if (mission == null) return BadRequest("Mission Id provided does not exist");

            var newSession = new Session
            {
                Mission = mission,
                MissionType = request.MissionType,
                TimeCreated = DateTime.Now,
                WorldName = request.WorldName
            };

            try
            {
                await _database.AddAsync(newSession);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert new session: {message}", ex.Message);
                return Problem("Failed to insert new session", null, 500, "Failed to insert new session");
            }

            return CreatedAtAction(nameof(GetSession), new {SessionId = newSession.SessionId},
                newSession.ToSessionResponse());
        }

        [HttpGet("{sessionId:int}")]
        [ProducesResponseType(typeof(SessionResponse), 200)]
        public async Task<IActionResult> GetSession(long sessionId)
        {
            var session = await _database.Sessions
                .AsNoTracking()
                .Include(x => x.Mission)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);

            if (session == null) return BadRequest("Session Id provided does not exist");

            return Ok(session.ToSessionResponse());
        }

        [HttpGet]
        [ProducesResponseType(typeof(SessionResponse[]), 200)]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _database.Sessions
                .AsNoTracking()
                .Include(x => x.Mission)
                .ToListAsync();

            return Ok(sessions.ToSessionResponseIEnumerable());
        }
        
        [Authorize]
        [HttpPut("{sessionId:int}")]
        public async Task<IActionResult> UpdateSession(long sessionId, [FromBody] UpdateSessionRequest request)
        {
            var session = await _database.Sessions
                .Include(x => x.Mission)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);

            if (session == null) return BadRequest("Session Id provided does not exist");

            if (request.MissionId.HasValue)
            {
                var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == request.MissionId);
                if (mission == null) return BadRequest("Mission Id provided does not exist");

                session.Mission = mission;
            }

            if (string.IsNullOrWhiteSpace(request.WorldName))
                session.WorldName = request.WorldName;
            if (request.TimeCreated.HasValue)
                session.TimeCreated = request.TimeCreated.Value;
            if (request.TimeEnded.HasValue)
                session.TimeEnded = request.TimeEnded.Value;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update session {sessionId}: {message}", sessionId, ex.Message);
                return Problem("Failed to update session. Most likely it has been modified or deleted while updating",
                    null, 500, "Failed to update session");
            }
            
            return Ok(session.ToSessionResponse());
        }

        [Authorize]
        [HttpPut("{sessionId:int}/endsession")]
        [ProducesResponseType(typeof(SessionResponse), 200)]
        public async Task<IActionResult> EndSession(long sessionId)
        {
            var session = await _database.Sessions
                .Include(x => x.Mission)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);
            if (session == null) return BadRequest("Session Id provided does not exist");
            
            session.TimeEnded = DateTime.Now;
            
            return Ok(session.ToSessionResponse());
        }

        [Authorize]
        [HttpDelete("{sessionId:int}")]
        [ProducesResponseType(null, 200)]
        public async Task<IActionResult> DeleteSession(long sessionId)
        {
            var session = await _database.Sessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);
            if (session == null) return BadRequest("Session Id provided does not exist");

            try
            {
                _database.Sessions.Remove(session);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove session {sessionId}: {message}", sessionId, ex.Message);
                return Problem("Failed to remove session. Maybe someone else has already deleted it.", null, 500,
                    "Failed to remove session");
            }

            return Ok();
        }
    }
}