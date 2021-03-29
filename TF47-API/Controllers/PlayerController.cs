using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly DatabaseContext _databaseContext;

        public PlayerController(ILogger<PlayerController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var response = await _databaseContext.Players
                .AsNoTracking()
                .ToListAsync();
            
            return Ok(response.ToPlayerResponseIEnumerable());
        }
        
        [AllowAnonymous]
        [HttpGet("{playerUid}")]
        public async Task<IActionResult> GetPlayer(string playerUid)
        {
            var response = await _databaseContext.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PlayerUid == playerUid);

            if (response == null) return BadRequest("Requested user does not exist");

            return Ok(response.ToPlayerResponse());
        }
        
        [HttpGet("details")]
        public async Task<IActionResult> GetPlayersDetails()
        {
            var response = _databaseContext.Players
                .AsNoTracking()
                .Include(x => x.PlayerChats)
                .Include(x => x.PlayerNotes)
                .AsEnumerable();

            return Ok(response.ToPlayerResponseWithDetailsIEnumerable());
        }
    }
}