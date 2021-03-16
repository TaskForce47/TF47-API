using System;
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
    public class IssueItemController : ControllerBase
    {
        private readonly ILogger<Issue> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public IssueItemController(
            ILogger<Issue> logger,
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;

        }

        [HttpPost("")]
        public async Task<IActionResult> CreateIssueItem([FromBody] CreateIssueItemRequest request)
        {
            var issueGroup =  await _database.Issues.FirstOrDefaultAsync(x => x.IssueId == request.IssueId);
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            if (issueGroup == null) return BadRequest("Issue does not exist");

            var issueItem = new IssueItem
            {
                Author = user,
                IsEdited = false,
                Message = request.Message,
                TimeCreated = DateTime.Now,
                TimeLastEdited = DateTime.Now
            };

            await _database.AddAsync(issueItem);
            await _database.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIssueItem), new {issueItemId = issueItem.IssueId},
                issueItem);
        }

        [HttpGet("{issueItemId:int}")]
        public async Task<IActionResult> GetIssueItem(int issueItemId)
        {
            var issueItem = await _database.IssueItems
                .FirstOrDefaultAsync(x => x.IssueItemId == issueItemId);
            return Ok(issueItem);
        }

        [HttpPut("{issueItemId:int}")]
        public async Task<IActionResult> UpdateIssueItem(int issueItemId, [FromBody] UpdateIssueItemRequest request)
        {
            var issueItem = await _database.IssueItems.FindAsync(issueItemId).AsTask();
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            if (issueItem == null)
                return BadRequest("IssueItem does not exist");

            if (user.UserId != issueItem.Author.UserId)
                return BadRequest("Only the user the wrote that created the item can edit it");

            issueItem.IsEdited = true;
            issueItem.TimeLastEdited = DateTime.Now;
            issueItem.Message = request.Message;

            await _database.SaveChangesAsync();

            return Ok(issueItem);
        }

        [HttpDelete("{issueItemId:int}")]
        public async Task<IActionResult> DeleteIssueItem(int issueItemId)
        {
            var issueItem = await _database.IssueItems.FindAsync(issueItemId);
            _database.Remove(issueItemId);
            await _database.SaveChangesAsync();
            return Ok();
        }
    }
}
