using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.Response;

namespace TF47_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly ILogger<MissionController> _logger;
        private readonly DatabaseContext _database;

        public MissionController(ILogger<MissionController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetMissions()
        {
            return await Task.Run(() =>
            {
                return Ok(
                    _database.Missions
                        .Include(x => x.Campaign)
                        .Select(x => new MissionResponse
                        {
                            MissionId = x.MissionId,
                            CampaignId = x.Campaign.CampaignId,
                            CampaignName = x.Campaign.Name,
                            MissionName = x.Name,
                            MissionType = x.MissionType
                        }));
            });
        }

        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetMission(uint id)
        {
            var mission = await _database.Missions
                .Include(x => x.Campaign)
                .FirstOrDefaultAsync(x => x.MissionId == id);

            if (mission == null) return NotFound("Mission does not exist");
            
            return Ok(new MissionResponse
            {
                MissionId = mission.MissionId,
                CampaignId = mission.Campaign.CampaignId,
                CampaignName = mission.Campaign.Name,
                MissionName = mission.Name,
                MissionType = mission.MissionType
            });
        }

        [HttpPut("create")]
        public async Task<IActionResult> CreateMission([FromBody] CreateMissionRequest request)
        {
            var campaign = await _database.Campaigns.FirstOrDefaultAsync(x => x.CampaignId == request.CampaignId);
            if (campaign == null) return NotFound("Campaign does not exist");

            var mission = new Mission
            {
                Campaign = campaign,
                MissionType = request.MissionType,
                Name = request.MissionName
            };

            await _database.Missions.AddAsync(mission);
            await _database.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMission(uint id)
        {
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == id);
            if (mission == null) return NotFound("Mission does not exist");

            _database.Remove(mission);
            await _database.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateMission(uint id, [FromBody] UpdateMissionRequest request)
        {
            var mission = await _database.Missions.FirstOrDefaultAsync(x => x.MissionId == id);
            if (mission == null) return NotFound("Mission does not exist");

            if (request.CampaignId != null)
            {
                var campaign = await _database.Campaigns.FirstOrDefaultAsync(x => x.CampaignId == request.CampaignId);
                if (campaign == null) return NotFound("Campaign does not exist");

                mission.Campaign = campaign;
            }

            if (request.MissionType != null)
                mission.MissionType = request.MissionType.Value;
            if (! string.IsNullOrEmpty(request.MissionName))
                mission.Name = request.MissionName;

            await _database.SaveChangesAsync();
            return Ok(mission);
        }

        [HttpGet("{id}/getSessions")]
        public async Task<IActionResult> GetSessions(uint id)
        {
            var mission = await _database.Missions
                .Include(x => x.Sessions)
                .FirstOrDefaultAsync(x => x.MissionId == id);
            if (mission == null) return NotFound("Mission does not exist");

            return Ok(mission.Sessions);
        }
        
        
    }
}
