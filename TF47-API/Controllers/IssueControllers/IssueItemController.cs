using System;
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
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueItemController : ControllerBase
    {
        private readonly ILogger<IssueItemController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public IssueItemController(
            ILogger<IssueItemController> logger,
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
        [ProducesResponseType(typeof(IssueItemResponse), 200)]
        public async Task<IActionResult> CreateIssueItem([FromBody] CreateIssueItemRequest request)
        {
            var issue =  await _database.Issues.FirstOrDefaultAsync(x => x.IssueId == request.IssueId);
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);

            _database.Attach(user);
            
            if (issue == null) return BadRequest("Issue does not exist");

            var issueItem = new IssueItem
            {
                Author = user,
                Issue = issue,
                IsEdited = false,
                Message = request.Message,
                TimeCreated = DateTime.Now,
                TimeLastEdited = null
            };

            await _database.IssueItems.AddAsync(issueItem);
            await _database.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIssueItem), new {issueItemId = issueItem.IssueId},
                issueItem.ToIssueItemResponse());
        }

        [HttpGet("{issueItemId:int}")]
        [ProducesResponseType(typeof(IssueItemResponse), 200)]
        public async Task<IActionResult> GetIssueItem(int issueItemId)
        {
            var issueItem = await _database.IssueItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IssueItemId == issueItemId);
            return Ok(issueItem.ToIssueItemResponse());
        }
        
        [Authorize]
        [HttpPut("{issueItemId:int}")]
        [ProducesResponseType(typeof(IssueItemResponse), 200)]
        public async Task<IActionResult> UpdateIssueItem(int issueItemId, [FromBody] UpdateIssueItemRequest request)
        {
            var issueItem = await _database.IssueItems.FindAsync(issueItemId);
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);

            if (issueItem == null)
                return BadRequest("IssueItem does not exist");

            if (user.UserId != issueItem.Author.UserId)
                return BadRequest("Only the user the wrote that created the item can edit it");

            issueItem.IsEdited = true;
            issueItem.TimeLastEdited = DateTime.Now;
            issueItem.Message = request.Message;

            await _database.SaveChangesAsync();

            return Ok(issueItem.ToIssueItemResponse());
        }
        
        [RequirePermission("issue:remove")]
        [Authorize]
        [HttpDelete("{issueItemId:int}")]
        public async Task<IActionResult> DeleteIssueItem(long issueItemId)
        {
            var issueItem = await _database.IssueItems.FindAsync(issueItemId);

            if (issueItem == null) return BadRequest("IssueItem provided does not exist");
            
            try
            {
                _database.Remove(issueItem);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove issueItem with id {id} from the database: {message}", issueItemId,
                    ex.Message);
                return Problem("Found issueItem in database but unable to remove it", null, 500,
                    "Failed to remove IssueItem");
            }

            return Ok();
        }
    }
}
