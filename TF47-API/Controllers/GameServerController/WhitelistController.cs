using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;
using TF47_API.Services;

namespace TF47_API.Controllers.GameServerController
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WhitelistController : Controller
    {
        private readonly ILogger<WhitelistController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public WhitelistController(
            ILogger<WhitelistController> logger, 
            DatabaseContext database, 
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        [AllowAnonymous]
        [HttpGet("whitelists")]
        [ProducesResponseType(typeof(WhitelistResponse), 200)]
        public async Task<IActionResult> GetWhitelists()
        {
            var response = await _database.Whitelists
                .AsNoTracking()
                .ToListAsync();
            return Ok(response.ToWhitelistResponseIEnumerable());
        }
        
        [RequirePermission("whitelist:view")]
        [HttpGet("user/{playerUid}")]
        [ProducesResponseType(typeof(UserWhitelistingResponse), 200)]
        public async Task<IActionResult> GetWhitelistingsUser(string playerUid)
        {
            var user = await _database.Players
                .AsNoTracking()
                .Include(x => x.PlayerWhitelistings)
                .FirstOrDefaultAsync(x => x.PlayerUid == playerUid);

            if (user == null)
                return BadRequest("User Uid provided does not exist");
            
            return Ok(user.ToPlayerWhitelistingResponse());
        }
        
        [RequirePermission("whitelist:view")]
        [HttpGet("users")]
        [ProducesResponseType(typeof(WhitelistResponse[]), 200)]
        public async Task<IActionResult> GetAllWhitelistings()
        {
            var users = await _database.Players
                .AsNoTracking()
                .Include(x => x.PlayerWhitelistings)
                .ToListAsync();
            return Ok(users.ToPlayerWhitelistingResponseIEnumerable());
        }

        [RequirePermission("whitelist:view")]
        [HttpGet("user/{playerUid}/nonMember")]
        [ProducesResponseType(typeof(WhitelistResponse[]), 200)]
        public async Task<IActionResult> GetWhitelistsUserNonMember(string playerUid)
        {
            var user = await _database.Players
                .AsNoTracking()
                .Include(x => x.PlayerWhitelistings)
                .FirstOrDefaultAsync(x => x.PlayerUid == playerUid);
            if (user == null) return BadRequest("User Uid provided does not exist");

            var whitelists = await _database.Whitelists
                .AsNoTracking()
                .Where(x => !user.PlayerWhitelistings.Contains(x))
                .ToListAsync();

            return Ok(whitelists.ToWhitelistResponseIEnumerable());
        }

        [RequirePermission("whitelist:addPlayer")]
        [HttpPut("userWhitelisting")]
        public async Task<IActionResult> WhitelistUser([FromBody] CreateUserWhitelistingRequest request)
        {
            var whitelist = await _database.Whitelists
                .Include(x => x.Players)
                .FirstOrDefaultAsync(x => x.WhitelistId == request.WhitelistId);
            if (whitelist == null)
                return BadRequest("Whitelist Id provided does not exist");

            var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
            if (player == null)
                return BadRequest("User Uid provided does not exist");

            if (whitelist.Players.All(x => x.PlayerUid != player.PlayerUid))
                whitelist.Players.Add(player);

            await _database.SaveChangesAsync();
            
            return Ok();
        }
        
        [RequirePermission("whitelist:addPlayer")]
        [HttpPut("userWhitelistingBatch")]
        public async Task<IActionResult> WhitelistUserBatch([FromBody] CreateUserWhitelistingRequest[] requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    var whitelist =
                        await _database.Whitelists
                            .Include(x => x.Players)
                            .FirstOrDefaultAsync(x => x.WhitelistId == request.WhitelistId);
                    if (whitelist == null)
                        throw new Exception("Whitelist Id provided does not exist");

                    var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
                    if (player == null)
                        throw new Exception("User Uid provided does not exist");

                    if (whitelist.Players.All(x => x.PlayerUid != player.PlayerUid))
                        whitelist.Players.Add(player);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Either a userUid or whitelist id does not exist!");
            }
            await _database.SaveChangesAsync();

            return Ok();
        }

        [RequirePermission("whitelist:removePlayer")]
        [HttpPut("removeWhitelisting")]
        public async Task<IActionResult> RemoveWhitelisting([FromBody] RemoveUserWhitelistingRequest request)
        {
            var whitelist = await _database.Whitelists
                .Include(x => x.Players)
                .FirstOrDefaultAsync(x => x.WhitelistId == request.WhitelistId);
            if (whitelist == null) return BadRequest("Whitelist Id provided does not exist");
            var user = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
            if (user == null) return BadRequest("User Uid provided does not exist");

            whitelist.Players.Remove(user);

            await _database.SaveChangesAsync();

            return Ok();
        }
    }
}