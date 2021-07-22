using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Controllers.GameServerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameServerController : Controller
    {
        private readonly ILogger<GameServerController> _logger;
        private readonly DatabaseContext _database;

        public GameServerController(
            ILogger<GameServerController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("{serverId}")]
        [ProducesResponseType(typeof(ServerResponse), 200)]
        public async Task<IActionResult> GetServer(int serverId)
        {
            var server = await _database.Servers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerID == serverId);

            if (server == null) return BadRequest("Server ID provided does not exist");

            return Ok(server.ToServerResponse());
        }
                

        [HttpGet]
        [ProducesResponseType(typeof(ServerResponse[]), 200)]
        public async Task<IActionResult> GetServers()
        {
            var servers = await _database.Servers
                .AsNoTracking()
                .ToListAsync();
            return Ok(servers.ToServerResponseIEnumerable());
        }
                
        [HttpPost]
        [ProducesResponseType(typeof(ServerResponse), 201)]
        public async Task<IActionResult> CreateServer([FromBody] CreateServerRequest request)
        {
            var server = new Server
            {
               Name = request.Name,
               Description = request.Description,
               IP = request.IP,
               Port = request.Port,
               Branch = request.Branch
            };

            try
            {
                await _database.Servers.AddAsync(server);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new server: {message}", ex.Message);
                return Problem(
                    "Failed to create new server.", null, 500, "Failed to create new server");
            }

            return CreatedAtAction(nameof(GetServer), new {ServerId = server.ServerID}, server.ToServerResponse());
        }
    }
}