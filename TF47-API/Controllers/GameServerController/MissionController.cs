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
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.Response;

namespace TF47_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly DatabaseContext _database;

        public MissionController(ILogger<MissionController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("{missionId:int}")]
        [ProducesResponseType(typeof(MissionResponse), 200)]
        public async Task<IActionResult> GetMission(long missionId)
        {
            var mission = await _database.Missions
                .AsNoTracking()
                .Include(x => x.Campaign)
                .FirstOrDefaultAsync(x => x.MissionId == missionId);

            if (mission == null) return BadRequest("Mission id provided does not exist");

            return Ok(mission.ToMissionResponse());
        }

        [HttpGet]
        [ProducesResponseType(typeof(MissionResponse[]), 200)]
        public async Task<IActionResult> GetMissions()
        {
            var missions = await _database.Missions
                .AsNoTracking()
                .Include(x => x.Campaign)
                .ToListAsync();

            return Ok(missions.ToMissionResponsesIEnumerable());
        }
        
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(MissionResponse), 201)]
        public async Task<IActionResult> CreateMission([FromBody] CreateMissionRequest request)
        {
            var campaign = await _database.Campaigns.FirstOrDefaultAsync(x => x.CampaignId == request.CampaignId);

            if (campaign == null) return BadRequest("Campaign provided does not exist");

            var newMission = new Mission
            {
                Name = request.Name,
                Description = request.Description,
                DescriptionShort = request.DescriptionShort,
                Campaign = campaign,
                CampaignId = campaign.CampaignId,
                MissionType = request.MissionType,
                SlottingTime = request.SlottingTime,
                BriefingTime = request.BriefingTime,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                RequiredDLCs = request.RequiredDLCs
            };

            try
            {
                await _database.AddAsync(newMission);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert new mission: {message}", ex.Message);
                return Problem("Failed to insert new mission object into database", null, 500,
                    "Failed to create mission");
            }

            return CreatedAtAction(nameof(GetMission), new {MissionId = newMission.MissionId},
                newMission.ToMissionResponse());
        }

        [Authorize]
        [HttpPut("{missionId:int}")]
        [ProducesResponseType(typeof(MissionResponse), 200)]
        public async Task<IActionResult> UpdateMission(long missionId, [FromBody] UpdateMissionRequest request)
        {
            var mission = await _database.Missions
                .FirstOrDefaultAsync(x => x.MissionId == missionId);

            if (mission == null) return BadRequest("MissionId provided does not exist");

            if (string.IsNullOrWhiteSpace(request.Name))
                mission.Name = request.Name;
            if (request.MissionType.HasValue)
                mission.MissionType = request.MissionType.Value;
            if (request.CampaignId.HasValue && mission.CampaignId != request.CampaignId)
            {
                var campaign = await _database.Campaigns.FirstOrDefaultAsync(x => x.CampaignId == request.CampaignId);
                if (campaign == null) return BadRequest("Campaign provided does not exist");

                mission.Campaign = campaign;
                mission.CampaignId = campaign.CampaignId;
            }

            mission.DescriptionShort = request.DescriptionShort;
            mission.SlottingTime = request.SlottingTime;
            mission.BriefingTime = request.BriefingTime;
            mission.StartTime = request.StartTime;
            mission.EndTime = request.EndTime;
            mission.RequiredDLCs = request.RequiredDLCs;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update mission {missionId}: {message}", missionId, ex.Message);
                return Problem("Failed to update mission. Maybe it has been removed or updated by another user.", null,
                    500, "Failed to update mission");
            }

            return Ok(mission.ToMissionResponse());
        }
        
        [Authorize]
        [HttpDelete("{missionId:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteMission(long missionId)
        {
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == missionId);

            if (mission == null) return BadRequest("Mission provided does not exist");

            try
            {
                _database.Missions.Remove(mission);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove mission {missionId}: {message}", missionId, ex.Message);
                return Problem("Failed to remove mission. Maybe it has been removed by someone else.", null, 500,
                    "Failed to remove mission");
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("{missionId:int}/slotting")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetSlotting(long missionId)
        {
            var mission = await _database.Missions.AsNoTracking().Include(x => x.SlotGroups).ThenInclude(x => x.Slots).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.MissionId == missionId);

            return Ok(mission.SlotGroups.ToSlotGroupResponseIEnumerable());
        }
    }
}