using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models;
using TF47_Backend.Database.Models.GameServer;
using TF47_Backend.Dto;
using TF47_Backend.Dto.RequestModels;

namespace TF47_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ILogger<CampaignController> _logger;
        private readonly DatabaseContext _database;

        public CampaignController(ILogger<CampaignController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetCampaigns()
        {
            return Ok(_database.Campaigns.Where(x => x.CampaignId > 0));
        }

        [HttpPut("create")]
        public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request)
        {
            var campaign = new Campaign
            {
                Description = request.Description,
                Name = request.Name,
                TimeCreated = DateTime.Now
            };
            await _database.AddAsync(campaign);
            await _database.SaveChangesAsync();
            return Ok(campaign);
        }
    }
}
