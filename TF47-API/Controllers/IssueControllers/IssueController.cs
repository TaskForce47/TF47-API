using System;
using System.Linq;
using System.Net;
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
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly ILogger<IssueController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public IssueController(
            ILogger<IssueController> logger,
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(IssueResponse), 201)]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueRequest request)
        {
            var issueGroup = await _database.IssueGroups.FindAsync(request.IssueGroupId);
            var issueTags = await _database.IssueTags
                .Where(x => request.Tags.Contains(x.IssueTagId))
                .ToListAsync();
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            if (issueGroup == null)
            {
                return BadRequest("Issue group does not exist");
            }

            var newIssue = new Issue
            {
                IsClosed = false,
                IssueCreator = user,
                IssueGroup = issueGroup,
                IssueTags = issueTags,
                TimeCreated = DateTime.Now,
                TimeLastUpdated = DateTime.Now,
                Title = request.Title
            };

            try
            {
                await _database.AddAsync(newIssue);
                await _database.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Issue does already exist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Something went wrong while trying to add the new Issue to the database",
                    null, 500, "Failed to create");


            }
            return CreatedAtAction(nameof(GetIssue), new {issueId = newIssue.IssueId},
                new IssueResponse(newIssue.IssueId, newIssue.Title, newIssue.IsClosed, newIssue.IssueCreator.UserId,
                    newIssue.IssueCreator.Username, newIssue.TimeCreated, newIssue.TimeLastUpdated, null,
                    issueTags.Select(x => new IssueTagResponse(x.IssueTagId, x.TagName, x.Color))));
        }

        [HttpGet("{issueId:int}")]
        [ProducesResponseType(typeof(IssueResponse), 200)]
        public async Task<IActionResult> GetIssue(int issueId)
        {
            var issue = await _database.Issues
                .AsNoTracking()
                .Select(x => new IssueResponse(x.IssueId, x.Title, x.IsClosed, x.IssueCreator.UserId,
                    x.IssueCreator.Username, x.TimeCreated, x.TimeLastUpdated,
                    x.IssueItems.Select(y => new IssueItemResponse(y.IssueItemId, y.Author.UserId, y
                        .Author.Username, y.Message, y.TimeCreated, y.TimeLastEdited)),
                    x.IssueTags.Select(y => new IssueTagResponse(y.IssueTagId, y.TagName, y.Color))))
                .FirstOrDefaultAsync(x => x.IssueId == issueId);

            return Ok(issue);
        }
        
        [Authorize]
        [HttpPut("{issueId:int}")]
        [ProducesResponseType(typeof(IssueResponse), 200)]
        public async Task<IActionResult> UpdateIssue(int issueId, [FromBody] UpdateIssueRequest request)
        {
            var issue = await _database.Issues.FindAsync(issueId);
            if (issue == null) return BadRequest("Issue does not exist");

            if (!string.IsNullOrEmpty(request.Title))
                issue.Title = request.Title;

            if (request.Tags.Length > 0)
            {
                var issueTags = await _database.IssueTags
                    .Where(x => request.Tags.Contains(x.IssueTagId))
                    .ToListAsync();
                issue.IssueTags = issueTags;
            }

            issue.TimeLastUpdated = DateTime.Now;
            //_database.Update(issue);
            await _database.SaveChangesAsync();

            return Ok(new IssueResponse(issue.IssueId, issue.Title, issue.IsClosed, issue.IssueCreator.UserId,
                issue.IssueCreator.Username, issue.TimeCreated, 
                issue.TimeLastUpdated, null, null));
        }
        
        [Authorize]
        [HttpDelete("{issueId:int}")]
        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            var issue = await _database.Issues.FindAsync(issueId);
            if (issue == null) return BadRequest("Issue does not exist");

            try
            {
                _database.Issues.Remove(issue);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete issue with id {id}: {message}", issueId, ex.Message);
                return Problem("Found the issue in the database but unable to delete id", null, 500,
                    "Failed to delete");
            }

            return Ok();
        }
    }
}
