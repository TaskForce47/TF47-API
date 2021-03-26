using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Dto.RequestModels;
using TF47_Backend.Dto.ResponseModels;
using TF47_Backend.Services;

namespace TF47_Backend.Controllers.ChangelogControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangelogController : Controller
    {
        private readonly ILogger<ChangelogController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public ChangelogController(
            ILogger<ChangelogController> logger, 
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(ChangelogResponse[]), 200)]
        public async Task<IActionResult> GetChangelogs()
        {
            var response = await Task.Run (() =>
                {
                    return _database.Changelogs
                        .Where(x => x.ChangelogId > 0)
                        .OrderByDescending(x => x.ChangelogId)
                        .Select(x => new ChangelogResponse(x.ChangelogId, x.Title, x.Tags, x.Text, x.TimeReleased));
                });
            return Ok(response);
        }

        [HttpGet("{changelogId:int}")]
        [ProducesResponseType(typeof(ChangelogResponse), 200)]
        public async Task<IActionResult> GetChangelog(int changelogId)
        {
            var changelog = await _database.Changelogs
                .FindAsync(changelogId);
            if (changelog == null)
                return BadRequest("Changelog requested does not exist");

            return Ok(changelog);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateChangelog([FromBody] CreateChangelogRequest request)
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);
            var changelog = new Changelog
            {
                Author = user,
                Tags = request.Tags,
                Text = request.Text,
                Title = request.Title,
                TimeReleased = DateTime.Now
            };
            try
            {
                await _database.AddAsync(changelog);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new changelog to database for user {user}: {message}", user.Username,
                    ex.Message);
                return Problem("Failed to save new changelog to database", null, 500, "Cannot create changelog");
            }
            return CreatedAtAction(nameof(GetChangelog), new {changelogId = changelog.ChangelogId},
                changelog);
        }
        
        [Authorize]
        [HttpPut("{changelogId:int}")]
        public async Task<IActionResult> UpdateChangelog(int changelogId, [FromBody] UpdateChangelogRequest request)
        {
            var changelog = await _database.Changelogs.FindAsync(changelogId);
            if (changelog == null)
                return BadRequest("Changelog requested does not exist");

            if (!string.IsNullOrWhiteSpace(request.Text))
                changelog.Text = request.Text;
            if (!string.IsNullOrWhiteSpace(request.Title))
                changelog.Title = request.Title;
            
            if (!request.IgnoreTags)
                changelog.Tags = request.Tags;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update changelog {changelogid}: {error}", changelogId, ex.Message);
                return Problem("Failed to update changelog changes to database", null, 500, "Failed to save changes");
            }

            return Ok(changelog);
        }
        

        [Authorize]
        [HttpDelete("{changelogId:int}")]
        public async Task<IActionResult> DeleteChangelog(int changelogId)
        {
            var changelog = await _database.Changelogs.FindAsync(changelogId);
            if (changelog == null)
                return BadRequest("Requested Changelog does not exist");

            try
            {
                _database.Changelogs.Remove(changelog);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove changelog entity id {changelogId}", changelogId);
                return Problem("Found the changelog in the database but failed to remove it.", null, 500,
                    "Failed to remove changelog");
            }

            return Ok();
        }
    }
}