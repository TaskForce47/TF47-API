using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Dto.RequestModels;
using TF47_Backend.Services;

namespace TF47_Backend.Controllers.IssueControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly ILogger<Issue> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public IssueController(
            ILogger<Issue> logger,
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueRequest request)
        {
            var issueGroup = await _database.IssueGroups.FindAsync(request.IssueGroupId).AsTask();
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
            return CreatedAtAction(nameof(GetIssue), new { issueId = newIssue.IssueId }, newIssue);
        }

        [HttpGet("{issueId:int}")]
        public async Task<IActionResult> GetIssue(int issueId)
        {
            var issue = await _database.Issues
                .FindAsync(issueId);
            return Ok(issue);
        }

        [HttpPut("{issueId:int}")]
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

            return Ok(issue);
        }

        [HttpDelete("{issueId:int}")]
        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            var issue = await _database.Issues.FindAsync(issueId);
            if (issue == null) return BadRequest("Issue does not exist");

            _database.Issues.Remove(issue);
            await _database.SaveChangesAsync();

            return Ok();
        }
    }
}
