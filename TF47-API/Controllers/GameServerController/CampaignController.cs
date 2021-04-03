using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using CampaignResponse = TF47_API.Dto.ResponseModels.CampaignResponse;

namespace TF47_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : Controller
    {
        private readonly ILogger<CampaignController> _logger;
        private readonly DatabaseContext _database;

        public CampaignController(
            ILogger<CampaignController> logger, 
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CampaignResponse[]), 200)]
        public async Task<IActionResult> GetCampaigns()
        {
            var campaigns = await _database.Campaigns
                .AsNoTracking()
                .Include(x => x.Missions)
                .ToListAsync();

            return Ok(campaigns.ToCampaignResponseIEnumerable());
        }

        [HttpGet("{campaignId:int}")]
        [ProducesResponseType(typeof(CampaignResponse), 200)]
        public async Task<IActionResult> GetCampaign(long campaignId)
        {
            var campaign = await _database.Campaigns
                .AsNoTracking()
                .Include(x => x.Missions)
                .FirstOrDefaultAsync(x => x.CampaignId == campaignId);

            if (campaign == null) return BadRequest("Campaign provided does not exist");

            return Ok(campaign.ToCampaignResponse());
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CampaignResponse), 200)]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            var newCampaign = new Campaign
            {
                Name = request.Name,
                Description = request.Description,
                TimeCreated = DateTime.Now
            };

            try
            {
                await _database.Campaigns.AddAsync(newCampaign);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new campaign: {message}", ex.Message);
                return Problem("Failed to insert new campaign into the database", null, 500,
                    "Failed to insert new campaign");
            }

            return CreatedAtRoute(nameof(GetCampaign), new {CampaignId = newCampaign.CampaignId},
                newCampaign.ToCampaignResponse());
        }

        [Authorize]
        [HttpPut("{campaignId:int}")]
        public async Task<IActionResult> UpdateCampaign(long campaignId, [FromBody] UpdateCampaignRequest request)
        {
            var campaign = await _database.Campaigns.FirstOrDefaultAsync(x => x.CampaignId == campaignId);

            if (campaign == null) return BadRequest("Campaign provided does not exist");

            if (!string.IsNullOrWhiteSpace(request.Description))
                campaign.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Name))
                campaign.Name = request.Name;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update campaign {id}: {message}", campaignId, ex.Message);
                return Problem(
                    "Failed to update campaign. Most likely another user has edited or deleted it at the same time",
                    null, 500, "Failed to update campaign");
            }

            return Ok(campaign.ToCampaignResponse());
        }
        
        
        [Authorize]
        [HttpDelete("{campaignId:int}")]
        public async Task<IActionResult> DeleteCampaign(long campaignId)
        {
            var campaign = await _database.Campaigns
                .FirstOrDefaultAsync(x => x.CampaignId == campaignId);

            if (campaign == null) return BadRequest("Campaign provided does not exist");

            try
            {
                _database.Campaigns.Remove(campaign);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete campaign {id} from database: {message}", campaign, ex.Message);
                return Problem(
                    "Failed to remove the campaign from the database. Most likely it already has been removed by somebody else.",
                    null, 500, "Failed to delete campaign");
            }

            return Ok();
        }
    }
}