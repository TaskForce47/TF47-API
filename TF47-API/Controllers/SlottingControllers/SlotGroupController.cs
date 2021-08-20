using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.Response;
using TF47_API.Filters;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SlotGroupController : ControllerBase
    {
        private readonly ILogger<SlotGroupController> _logger;
        private readonly DatabaseContext _database;

        public SlotGroupController(
            ILogger<SlotGroupController> logger, 
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }
        
        [RequirePermission("slotgroup:create")]
        [HttpPost("")]
        [ProducesResponseType(typeof(SlotGroupResponse), 200)]
        public async Task<IActionResult> CreateSlotGroup([FromBody] CreateSlotGroupRequest request)
        {
            var mission = await _database.Missions.FindAsync(request.MissionId);
            if (mission == null) return BadRequest("MissionId provided does not match a mission");
            var newSlotGroup = new SlotGroup
            {
                Title = request.Title,
                Description = request.Description,
                MissionId = request.MissionId,
                OrderNumber = 0
            };

            try
            {
                await _database.SlotGroups.AddAsync(newSlotGroup);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new slot group to database: {message}", ex.Message);
                return Problem(
                    "Failed to add new slot group to database. Maybe there is something wrong about the session or properties are set incorrectly",
                    null, 500, "Failed to add new slot group");
            }

            return Ok(newSlotGroup.ToSlotGroupResponse());
        }

        [RequirePermission("slotgroup:update")]
        [HttpPut("{slotGroupId:int}")]
        [ProducesResponseType(typeof(SlotGroupResponse), 200)]
        public async Task<IActionResult> UpdateSlotGroup(long slotGroupId, [FromBody] UpdateSlotGroupRequest request)
        {
            var slotGroup = await _database.SlotGroups
                .FirstOrDefaultAsync(x => x.SlotGroupId == slotGroupId);
            if (slotGroup == null) return BadRequest("Requested slot group does not exist");
            
            if (!string.IsNullOrWhiteSpace(request.Title))
                slotGroup.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Description))
                slotGroup.Description = request.Description;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not update note {slotGroupId}: {message}", slotGroupId, ex.Message);
                return Problem("Could not update slot group. Maybe someone else deleted or updated at the same time", null,
                    500, "Failed to update slot group");
            }

            return Ok(slotGroup.ToSlotGroupResponse());
        }
        
        [RequirePermission("slotgroup:view")]
        [HttpGet("{slotGroupId:int}")]
        [ProducesResponseType(typeof(SlotGroupResponse), 200)]
        public async Task<IActionResult> GetSlotGroup(long slotGroupId)
        {
            var slotGroup = await _database.SlotGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SlotGroupId == slotGroupId);

            if (slotGroup == null) return BadRequest("Requested slot group does not exist");
             
            return Ok(slotGroup.ToSlotGroupResponse());
        }

        [RequirePermission("slotgroup:view")]
        [HttpGet("{slotGroupId:int}/slots")]
        [ProducesResponseType(typeof(SlotResponse[]), 200)]
        public async Task<IActionResult> GetSlots(long slotGroupId)
        {
            var slotGroup = await _database.SlotGroups
                .AsNoTracking()
                .Include(x => x.Slots)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.SlotGroupId == slotGroupId);

            if (slotGroup == null) return BadRequest("Requested slot group does not exist");

            return Ok(slotGroup.Slots.ToSlotResponseIEnumerable());
        }

        [RequirePermission("slotgroup:view")]
        [HttpGet("")]
        [ProducesResponseType(typeof(SlotGroupResponse[]), 200)]
        public async Task<IActionResult> GetSlotGroups()
        {
            var slotGroups = await Task.Run(() =>
            {
                return _database.SlotGroups
                    .AsNoTracking();
            });

            return Ok(slotGroups.AsEnumerable().ToSlotGroupResponseIEnumerable());
        }
        

        [RequirePermission("slotgroup:remove")]
        [HttpDelete("{slotGroupId:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteSlotGroup(long slotGroupId)
        {
            var slotGroup = await _database.SlotGroups.FindAsync(slotGroupId);
            if (slotGroup == null) return BadRequest("SlotGroupId provided does not match a Slot Group");

            try
            {
                _database.SlotGroups.Remove(slotGroup);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not delete slot group from database: {message}", ex.Message);
                return Problem(
                    "Could not delete slot group from database. Either it is has already been deleted by someone else or data could have been modified",
                    null, 500, "Slot group could not be deleted");
            }

            return Ok();
        }
    }
}