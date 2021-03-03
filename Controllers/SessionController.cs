using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models;
using TF47_Backend.Database.Models.GameServer;
using TF47_Backend.Dto;
using TF47_Backend.Dto.Response;

namespace TF47_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly DatabaseContext _database;

        public SessionController(ILogger<SessionController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetSessions()
        {
            return await Task.Run(() =>
            {
                var sessionResponses = _database.Sessions
                    .Include(x => x.Mission)
                    .Where(x => x.SessionId > 0)
                    .Select(x => new SessionResponse
                {
                    SessionId = x.SessionId,
                    MissionId = x.Mission.MissionId,
                    MissionName = x.Mission.Name,
                    SessionCreated = x.SessionCreated,
                    SessionEnded = x.SessionEnded
                });
                return Ok(sessionResponses);
            });
        }
        
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetSession(uint id)
        {

            var session = await _database.Sessions
                .Include(x => x.Mission)
                .FirstOrDefaultAsync(x => x.SessionId == id);

            if (session == null) return NotFound("Session does not exist");

            return Ok(new SessionResponse
            {
                MissionId = session.Mission.MissionId,
                MissionName = session.Mission.Name,
                SessionCreated = session.SessionCreated,
                SessionEnded = session.SessionEnded,
                SessionId = session.SessionId
            });
        }

        [HttpPut("create")]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == request.MissionId);
            if (mission == null) return NotFound("Mission id does not exist");
            var session = new Session
            {
                Mission = mission,
                MissionType = request.MissionType,
                SessionCreated = DateTime.Now,
                WorldName = request.WorldName
            };
            await _database.Sessions.AddAsync(session);
            await _database.SaveChangesAsync();

            return Ok(new
            {
                session_id = session.SessionId
            });
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteSession(uint id)
        {
            var session = await _database.Sessions.FirstOrDefaultAsync(x => x.SessionId == id);
            if (session == null) return NotFound("Session does not exist");
            _database.Remove(session);
            await _database.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateSession(uint id, [FromBody] UpdateSessionRequest request)
        {
            var session = await _database.Sessions.FirstOrDefaultAsync(x => x.SessionId == id);
            if (session == null) return NotFound("Session does not exist");
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == request.MissionId);
            if (mission == null) return NotFound("Mission id does not exist");


            session.SessionEnded = request.SessionEnded;
            session.WorldName = request.WorldName;
            session.Mission = mission;
            session.MissionType = request.MissionType;

            await _database.SaveChangesAsync();

            return Ok(session);
        }
    }
}
