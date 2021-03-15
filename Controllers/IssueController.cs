using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Dto.RequestModels;
using TF47_Backend.Services;

namespace TF47_Backend.Controllers
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
        public async Task<IActionResult> CreateIssue([FromBody]CreateIssueRequest request)
        {
            var issueGroupTask = _database.IssueGroups.FirstOrDefaultAsync(x => x.IssueGroupId == request.IssueGroupId);
            var userTask = _userProviderService.GetDatabaseUser(HttpContext);

            await Task.WhenAll(issueGroupTask, userTask);

            if (issueGroupTask.Result == null)
            {
                return BadRequest("Issue group does not exist");
            }

            var newIssue = new Issue
            { 
                IsClosed = false,
                IssueCreator = userTask.Result,
                IssueGroup = issueGroupTask.Result,
                Tags = request.Tags,
                TimeCreated = DateTime.Now,
                TimeLastUpdated = DateTime.Now,
                Title = request.Title
            };

            await _database.AddAsync(newIssue);
            await _database.SaveChangesAsync();

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
                issue.Tags = request.Tags;

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
