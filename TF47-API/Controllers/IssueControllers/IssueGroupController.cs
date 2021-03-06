﻿using System;
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
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;
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
        
        [RequirePermission("issue:create")]
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
                newIssueGroup.ToIssueGroupResponse());
        }

        [HttpGet("{issueGroupId}/")]
        [ProducesResponseType(typeof(IssueGroupResponse), 200)]
        public async Task<IActionResult> GetIssueGroup(int issueGroupId)
        {
            var issueGroup = await _database.IssueGroups
                .AsNoTracking()
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueTags)
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueCreator)
                .Include(x => x.Issues)
                .ThenInclude(x => x.IssueItems)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.IssueGroupId == issueGroupId);

            return Ok(issueGroup.ToIssueGroupResponse());
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IssueGroupResponse[]), 200)]
        public async Task<IActionResult> GetIssueGroups()
        {
            var issueGroups = await Task.Run(() =>
            {
                return _database.IssueGroups
                    .AsNoTracking()
                    .AsNoTracking()
                    .Include(x => x.Issues)
                    .ThenInclude(x => x.IssueTags)
                    .Include(x => x.Issues)
                    .ThenInclude(x => x.IssueCreator)
                    .Include(x => x.Issues)
                    .ThenInclude(x => x.IssueItems)
                    .ThenInclude(x => x.Author)
                    .ToIssueGroupResponseIEnumerable();
            });
            return Ok(issueGroups);
        }
    }
}
