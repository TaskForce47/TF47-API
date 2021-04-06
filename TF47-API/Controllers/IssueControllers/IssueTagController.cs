using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;

namespace TF47_API.Controllers.IssueControllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueTagController : ControllerBase
    {
        private readonly ILogger<IssueTagController> _logger;
        private readonly DatabaseContext _database;

        public IssueTagController(
            ILogger<IssueTagController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }
        
        [RequirePermission("issue:create")]
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(IssueTagResponse), 201)]
        public async Task<IActionResult> CreateIssueTag(CreateIssueTagRequest request)
        {
            var issueTag = new IssueTag
            {
                Color = request.Color,
                TagName = request.TagName
            };

            try
            {
                await _database.IssueTags.AddAsync(issueTag);
                await _database.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning("User tried to insert a already existing key");
                return BadRequest("Issue Tag does already exist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("could not add the tag to the database ",
                    null, 500, "Failed to create tag");
            }

            return CreatedAtAction(nameof(GetIssueTag), new {issueTagId = issueTag.IssueTagId}, 
                issueTag.ToIssueTagResponse());
        }
        
        [RequirePermission("issue:update")]
        [Authorize]
        [HttpPut("{issueTagId:int}")]
        [ProducesResponseType(typeof(IssueTagResponse), 200)]
        public async Task<IActionResult> UpdateIssueTag(int issueTagId, [FromBody] UpdateIssueTagRequest request)
        {
            var issueTag = await _database.IssueTags.FindAsync(issueTagId);
            if (issueTag == null) return BadRequest("Issue tag does not exist");

            if (string.IsNullOrEmpty(request.Color))
                issueTag.Color = request.Color;

            if (string.IsNullOrEmpty(request.TagName))
                issueTag.TagName = request.TagName;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("could not store the new properties for issue tag",
                    null, 500, "Failed to update");
            }

            return Ok(issueTag.ToIssueTagResponse());
        }

        [HttpGet("{issueTagId:int}")]
        [ProducesResponseType(typeof(IssueTagResponse), 200)]
        public async Task<IActionResult> GetIssueTag(int issueTagId)
        {
            var issueTag = await _database.IssueTags.FindAsync(issueTagId);

            if (issueTag == null) return Ok();

            return Ok(issueTag.ToIssueTagResponse());
        }

        [HttpGet("")]
        public async Task<IActionResult> GetIssueTags()
        {
            var issueTags = await _database.IssueTags
                .ToListAsync();
            return Ok(issueTags.AsEnumerable().ToIssueTagResponseIEnumerable());
        }
        
        [RequirePermission("issue:delete")]
        [HttpDelete("{issueTagId:int}")]
        public async Task<IActionResult> RemoveIssueTag(int issueTagId)
        {
            var issueTag = await _database.IssueTags.FindAsync(issueTagId);

            if (issueTag == null) return BadRequest("Issue Tag does not exist");

            try
            {
                _database.Remove(issueTag);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("failed to delete the issue from the database",
                    null, 500, "Failed to delete");
            }

            return Ok();
        }
    }
}
