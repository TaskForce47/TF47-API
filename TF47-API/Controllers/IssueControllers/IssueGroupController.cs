using System;
using System.Linq;
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

namespace TF47_API.Controllers.IssueControllers
{
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
        
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(IssueGroupResponse), 201)]
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
                new IssueGroupResponse(newIssueGroup.IssueGroupId, newIssueGroup.GroupName,
                    newIssueGroup.GroupDescription, newIssueGroup.TimeGroupCreated, null));
        }

        [HttpGet("{issueGroupId}/")]
        [ProducesResponseType(typeof(IssueGroupResponse), 200)]
        public async Task<IActionResult> GetIssueGroup(int issueGroupId)
        {
            var issueGroup = await _database.IssueGroups
                .AsNoTracking()
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueItems)
                .Select(x =>
                    new IssueGroupResponse(x.IssueGroupId, x.GroupName, x.GroupDescription, x.TimeGroupCreated, 
                        x.Issues.Select(y => new IssueResponse(y.IssueId, y.Title, y.IsClosed, 
                            y.IssueCreator.UserId, y.IssueCreator.Username, y.TimeCreated, y.TimeLastUpdated, 
                            y.IssueItems
                                .Select(z => new IssueItemResponse(z.IssueItemId, z.Author.UserId,
                                z.Author.Username, z.Message, z.TimeCreated, z.TimeLastEdited)), 
                            y.IssueTags
                                .Select(z => new IssueTagResponse(z.IssueTagId, z.TagName, z.Color))))))
                .FirstOrDefaultAsync(x => x.IssueGroupId == issueGroupId);
            return Ok(issueGroup);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IssueGroupResponse[]), 200)]
        public async Task<IActionResult> GetIssueGroups()
        {
            var issueGroups = await _database.IssueGroups
                .AsNoTracking()
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueItems)
                .Select(x =>
                    new IssueGroupResponse(x.IssueGroupId, x.GroupName, x.GroupDescription, x.TimeGroupCreated, 
                        x.Issues.Select(y => new IssueResponse(y.IssueId, y.Title, y.IsClosed, 
                            y.IssueCreator.UserId, y.IssueCreator.Username, y.TimeCreated, y.TimeLastUpdated, 
                            y.IssueItems
                                .Select(z => new IssueItemResponse(z.IssueItemId, z.Author.UserId,
                                    z.Author.Username, z.Message, z.TimeCreated, z.TimeLastEdited)), 
                            y.IssueTags
                                .Select(z => new IssueTagResponse(z.IssueTagId, z.TagName, z.Color))))))
                .ToArrayAsync();
            return Ok(issueGroups);
        }
    }
}
