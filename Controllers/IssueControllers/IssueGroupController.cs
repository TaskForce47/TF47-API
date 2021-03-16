using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Dto.RequestModels;
using TF47_Backend.Services;

namespace TF47_Backend.Controllers.IssueControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueGroupController : ControllerBase
    {
        private readonly ILogger<IssueGroupController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public IssueGroupController(
            ILogger<IssueGroupController> logger,
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateIssueGroup([FromBody] CreateIssueGroupRequest request)
        {
            var newIssueGroup = new IssueGroup
            {
                GroupDescription = request.GroupDescription,
                GroupName = request.GroupName,
                TimeGroupCreated = DateTime.Now
            };

            try
            {
                await _database.AddAsync(newIssueGroup);
                await _database.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Issue Group does already exist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Something went wrong while trying to add the new Issue Group to the database",
                    null, 500, "Failed to create");
            }

            return CreatedAtAction(nameof(GetIssueGroup), new {issueGroupId = newIssueGroup.IssueGroupId},
                newIssueGroup);
        }

        [HttpGet("{issueGroupId}/")]
        public async Task<IActionResult> GetIssueGroup(int issueGroupId)
        {
            var issueGroup = await _database.IssueGroups
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueItems)
                .FirstOrDefaultAsync(x => x.IssueGroupId == issueGroupId);
            return Ok(issueGroup);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetIssueGroups()
        {
            var issueGroups = await _database.IssueGroups
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueItems)
                .ToArrayAsync();
            return Ok(issueGroups);
        }
    }
}
