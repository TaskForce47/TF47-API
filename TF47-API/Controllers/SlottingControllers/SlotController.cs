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
    public class SlotController : ControllerBase
    {
        private readonly ILogger<SlotController> _logger;
        private readonly DatabaseContext _database;

        public SlotController(
            ILogger<SlotController> logger, 
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }
        
        [RequirePermission("slot:create")]
        [HttpPost("")]
        [ProducesResponseType(typeof(SlotGroupResponse), 200)]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
        {
            var slotGroup = await _database.SlotGroups.FindAsync(request.SlotGroupId);
            if (slotGroup == null) return BadRequest("SlotGroupId provided does not match a slot group");
            var newSlot = new Slot
            {
                Title = request.Title,
                Description = request.Description,
                SlotGroupId = request.SlotGroupId,
                OrderNumber = request.OrderNumber,
                Difficulty = request.Difficulty,
                Reserve = request.Reserve,
                Blocked = request.Blocked,
                RequiredDLC = request.RequiredDLC
            };

            try
            {
                await _database.Slots.AddAsync(newSlot);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new slot to database: {message}", ex.Message);
                return Problem(
                    "Failed to add new slot to database. Maybe there is something wrong about the session or properties are set incorrectly",
                    null, 500, "Failed to add new slot");
            }

            return Ok(newSlot.ToSlotResponse());
        }

        [RequirePermission("slot:update")]
        [HttpPut("{slotId:int}")]
        [ProducesResponseType(typeof(SlotResponse), 200)]
        public async Task<IActionResult> UpdateSlot(long slotId, [FromBody] UpdateSlotRequest request)
        {
            var slot = await _database.Slots
                .FirstOrDefaultAsync(x => x.SlotId == slotId);
            if (slot == null) return BadRequest("Requested slot does not exist");
            
            if (!string.IsNullOrWhiteSpace(request.Title))
                slot.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Description))
                slot.Description = request.Description;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not update slot {slot}: {message}", slot, ex.Message);
                return Problem("Could not update slot. Maybe someone else deleted or updated at the same time", null,
                    500, "Failed to update slot");
            }

            return Ok(slot.ToSlotResponse());
        }
        
        [RequirePermission("slot:view")]
        [HttpGet("{slotId:int}")]
        [ProducesResponseType(typeof(SlotResponse), 200)]
        public async Task<IActionResult> GetSlotGroup(long slotId)
        {
            var slot = await _database.Slots
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SlotId == slotId);

            if (slot == null) return BadRequest("Requested slot does not exist");
             
            return Ok(slot.ToSlotResponse());
        }      

        [RequirePermission("slot:remove")]
        [HttpDelete("{slotId:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteSlotGroup(long slotId)
        {
            var slot = await _database.Slots.FindAsync(slotId);
            if (slot == null) return BadRequest("SlotId provided does not match a Slot");

            try
            {
                _database.Slots.Remove(slot);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not delete slot from database: {message}", ex.Message);
                return Problem(
                    "Could not delete slot from database. Either it is has already been deleted by someone else or data could have been modified",
                    null, 500, "Slot could not be deleted");
            }

            return Ok();
        }
    }
}