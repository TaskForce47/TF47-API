using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Services;
using TF47_API.Services.SquadManager;
using CreateSquadMemberRequest = TF47_API.Dto.RequestModels.CreateSquadMemberRequest;

namespace TF47_API.Controllers.SquadXml
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SquadMemberController : Controller
    {
        private readonly ILogger<SquadMemberController> _logger;
        private readonly DatabaseContext _database;
        private readonly ISquadManagerService _squadManagerService;
        private readonly IUserProviderService _userProviderService;

        public SquadMemberController(
            ILogger<SquadMemberController> logger, 
            DatabaseContext database,
            ISquadManagerService squadManagerService,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _squadManagerService = squadManagerService;
            _userProviderService = userProviderService;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SquadMemberResponse), 201)]
        public async Task<IActionResult> CreateSquadMember([FromBody] CreateSquadMemberRequest request)
        {
            var user = await _database.Users.FindAsync(request.UserId);
            if (user == null) return BadRequest("Provided userid cannot be found in the database");
            var squad = await _database.Squads
                .Include(x => x.SquadMembers)
                .FirstOrDefaultAsync(x => x.SquadId == request.SquadId );
            if (squad == null) return BadRequest("Provided squadid cannot be found in the database");
            if (squad.SquadMembers.Any(x => x.UserId == request.UserId))
                return BadRequest("Users is already a member of the squad");
            
            var squadMember = new SquadMember
            {
                Squad = squad,
                Mail = request.Mail,
                Remark = request.Remark,
                User = user
            };
            try
            {
                await _database.SquadMembers.AddAsync(squadMember);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert SquadMember {squadMemberName} into {squadName}, {message}",
                    user.Username, squad.Name, ex.Message);
                return Problem("Unable to save new SquadMember to the database", null, 500,
                    "Failed to update changes");
            }

            await _squadManagerService.WriteSquadXml(squad.SquadId, CancellationToken.None);
            
            return CreatedAtAction(nameof(GetSquadMember), new {squadMemberId = squadMember.SquadMemberId},
                new SquadMemberResponse(squadMember.SquadMemberId, squadMember.Remark, squadMember.Mail,
                    squadMember.UserId, squadMember.User.Username, squadMember.User.SteamId));
        }
        
        [HttpGet("{squadMemberId:int}")]
        [ProducesResponseType(typeof(SquadMemberResponse), 200)]
        public async Task<IActionResult> GetSquadMember(long squadMemberId)
        {
            var squadMember = await _database.SquadMembers
                .Include(x => x.User)
                .Select(x =>
                    new SquadMemberResponse(x.SquadMemberId, x.Remark, x.Mail, x.UserId, x.User.Username,
                        x.User.SteamId))
                .FirstOrDefaultAsync(x => x.SquadMemberId == squadMemberId);

            if (squadMember == null) return BadRequest("Requested squadMember does not exist");

            return Ok(squadMember);
        }
        
        [HttpPut("{squadMemberId:int}")]
        [ProducesResponseType(typeof(SquadMemberResponse), 200)]
        public async Task<IActionResult> UpdateSquadMember(long squadMemberId, UpdateSquadMemberRequest request)
        {
            var squadMember = await _database.SquadMembers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.SquadMemberId == squadMemberId);

            if (squadMember == null) return BadRequest("Requested squadMember does not exist");

            if (string.IsNullOrWhiteSpace(request.Mail))
                squadMember.Mail = request.Mail;
            if (string.IsNullOrWhiteSpace(request.Remark))
                squadMember.Remark = request.Remark;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update SquadMember {squadMemberId}:{squadMemberName}, {message}",
                    squadMemberId, squadMember.User.Username, ex.Message);
                return Problem("Found SquadMember in the database but unable to save changes", null, 500,
                    "Failed to update changes");
            }
            
            await _squadManagerService.WriteSquadXml(squadMember.SquadId, CancellationToken.None);

            return Ok(new SquadMemberResponse(squadMember.SquadMemberId, squadMember.Remark, squadMember.Mail,
                squadMember.UserId, squadMember.User.Username, squadMember.User.SteamId));
        }
        
        [HttpPut("{squadMemberId:int}/me")]
        public async Task<IActionResult> UpdateSquadMemberSelf(long squadMemberId, UpdateSquadMemberRequest request)
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);
            var squadMember = await _database.SquadMembers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.SquadMemberId == squadMemberId);

            if (squadMember == null) return BadRequest("Requested squadMember does not exist");
            if (squadMember.UserId != user.UserId) return BadRequest("You can only edit your own profile");

            if (string.IsNullOrWhiteSpace(request.Mail))
                squadMember.Mail = request.Mail;
            if (string.IsNullOrWhiteSpace(request.Remark))
                squadMember.Remark = request.Remark;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update SquadMember {squadMemberId}:{squadMemberName}, {message}",
                    squadMemberId, squadMember.User.Username, ex.Message);
                return Problem("Found SquadMember in the database but unable to save changes", null, 500,
                    "Failed to update changes");
            }
            
            await _squadManagerService.WriteSquadXml(squadMember.SquadId, CancellationToken.None);

            return Ok(new SquadMemberResponse(squadMember.SquadMemberId, squadMember.Remark, squadMember.Mail,
                squadMember.UserId, squadMember.User.Username, squadMember.User.SteamId));
        }
        
        [HttpDelete("{squadMemberId:int}")]
        public async Task<IActionResult> RemoveSquadMember(long squadMemberId)
        {
            var squadMember = await _database.SquadMembers.FindAsync(squadMemberId);

            if (squadMember == null) return BadRequest("Requested squadMember does not exist");

            try
            {
                _database.SquadMembers.Remove(squadMember);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove squadmember {squadMemberId} from squad {squadId} from database, {message}",
                    squadMember.UserId, squadMember.SquadId, ex.Message);
                return Problem("Found squadMember in the database but unable to remove him", null, 500,
                    "Failed to remove squadMember");
            }

            return Ok();
        } 

        [HttpDelete("{squadMemberId:int}/me")]
        public async Task<IActionResult> RemoveSquadMemberSelf(long squadMemberId)
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);
            var squadMember = await _database.SquadMembers.FindAsync(squadMemberId);
            
            if (squadMember == null) return BadRequest("Requested squadMember does not exist");
            if (squadMember.UserId != user.UserId) return BadRequest("You can only edit your own Profile");

            try
            {
                _database.SquadMembers.Remove(squadMember);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove squadmember {squadMemberId} from squad {squadId} from database, {message}",
                    squadMember.UserId, squadMember.SquadId, ex.Message);
                return Problem("Found squadMember in the database but unable to remove him", null, 500,
                    "Failed to remove squadMember");
            }

            return Ok();
        }
    }
}
