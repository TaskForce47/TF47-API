using System;
using System.Linq;
using System.Threading;
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
using TF47_API.Services.Authorization;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;
        private readonly IGroupPermissionCache _groupPermissionCache;

        public GroupController(
            ILogger<GroupController> logger, 
            DatabaseContext database,
            IUserProviderService userProviderService,
            IGroupPermissionCache groupPermissionCache)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
            _groupPermissionCache = groupPermissionCache;
        }
    
        [HttpGet]
        [ProducesResponseType(typeof(GroupResponse[]), 200)]
        public async Task<IActionResult> GetGroups()
        {
            var result = await _database.Groups
                .AsNoTracking()
                .Include(x => x.Users)
                .Include(x => x.Permissions)
                .ToListAsync();
            return Ok(result.ToGroupResponseIEnumerable());
        }

        [HttpGet("{groupId:int}")]
        [ProducesResponseType(typeof(GroupResponse), 200)]
        public async Task<IActionResult> GetGroup(int groupId)
        {

            var result = await _database.Groups
                .AsNoTracking()
                .Include(x => x.Users)
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.GroupId == groupId);

            if (result == null) return BadRequest("Request group does not exist");

            return Ok(result.ToGroupResponse()); 
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(GroupResponse[]), 200)]
        public async Task<IActionResult> GetGroupsSelf()
        {
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);
            var result = await _database.Groups
                .AsNoTracking()
                .Include(x => x.Users)
                .Include(x => x.Permissions)
                .Where(x => x.Users.Any(y => y.UserId == user.UserId))
                .ToListAsync();
            return Ok(result.ToGroupResponseIEnumerable());
        }
        
        [RequirePermission("group:create")]
        [HttpPost]
        [ProducesResponseType(typeof(GroupResponse), 201)]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var permissions = await _database.Permissions
                .Where(x => request.Permissions.Contains(x.PermissionId))
                .ToListAsync();
            
            var newGroup = new Group
            {
                BackgroundColor = request.BackgroundColor,
                TextColor = request.TextColor,
                Description = request.Description,
                IsVisible = request.IsVisible,
                Name = request.Name,
                Permissions = permissions
            };
            try
            {
                await _database.Groups.AddAsync(newGroup);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new group: {message}", ex.Message);
                return Problem("Failed to create new group", null, 500, "Failed to create new group");
            }

            await _groupPermissionCache.RefreshCache();
            
            return CreatedAtAction(nameof(GetGroup), new { GroupId = newGroup.GroupId }, newGroup.ToGroupResponse());
        }

        [RequirePermission("group:update")]
        [HttpPut("{groupId:int}")]
        [ProducesResponseType(typeof(GroupResponse), 200)]
        public async Task<IActionResult> UpdateGroupRequest(long groupId, [FromBody] UpdateGroupRequest request)
        {
            var group = _database.Groups
                .Include(x => x.Users)
                .FirstOrDefault(x => x.GroupId == groupId);

            if (group == null) return BadRequest("GroupId provided does not exist");

            var permissions = await _database.Permissions
                .Where(x => request.Permissions.Contains(x.PermissionId))
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(request.Description))
                group.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Name))
                group.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.BackgroundColor))
                group.BackgroundColor = request.BackgroundColor;
            if (!string.IsNullOrWhiteSpace(request.TextColor))
                group.TextColor = request.TextColor;
            if (request.IsVisible.HasValue)
                group.IsVisible = request.IsVisible.Value;

            group.Permissions = permissions;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update group {groupId}: {message}", groupId, ex.Message);
                return Problem("Failed to update group", null, 500, "Failed to update group");
            }
            
            await _groupPermissionCache.RefreshCache();

            return Ok(group.ToGroupResponse());
        }

        [RequirePermission("group:delete")]
        [HttpDelete("{groupId:int}")]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> DeleteGroup(long groupId)
        {
            var group = _database.Groups.FirstOrDefault(x => x.GroupId == groupId);
            if (group == null) return BadRequest("GroupId provided does not exist");

            try
            {
                _database.Groups.Remove(group);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove group {groupId}: {message}", ex.Message);
                return Problem("Failed to remove group. Most likely it has been already deleted.", null, 500,
                    "Failed to delete group");
            }

            return Ok();
        }
        
        [RequirePermission("group:adduser")]
        [HttpPost("{groupId:int}/addUser/{userId:Guid}")]
        [ProducesResponseType(typeof(GroupResponse), 200)]
        public async Task<IActionResult> AddUser(long groupId, Guid userId)
        {
            var group = _database.Groups
                .Include(x => x.Users)
                .FirstOrDefault(x => x.GroupId == groupId);
            if (group == null) return BadRequest("GroupId provided does not exist");
            
            var user = _database.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) return BadRequest("UserId provided does not exist");

            if (group.Users.Any(x => x.UserId == user.UserId))
                return BadRequest("User is already a member of the group");
            
            group.Users.Add(user);

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add user {userId} to group {groupId}: {message}", user.UserId, groupId, ex.Message);
                return Problem(
                    "Failed to add user to group. Most likely the users has already been added by someone else at the same time",
                    null, 500, "User cannot be added to group");
            }

            return Ok(group.ToGroupResponse());
        }
        
        [RequirePermission("group:removeuser")]
        [HttpDelete("{groupId:int}/removeUser/{userId:Guid}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteUser(long groupId, Guid userId)
        {
            var group = _database.Groups
                .Include(x => x.Users)
                .FirstOrDefault(x => x.GroupId == groupId);
            if (group == null) return BadRequest("GroupId provided does not exist");
            
            var user = _database.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) return BadRequest("UserId provided does not exist");

            group.Users.Remove(user);

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove user {userId} from group {groupId}: {message}", user.UserId, groupId, ex.Message);
                return Problem(
                    "Failed to remove user to group. Most likely the users has already been removed by someone else at the same time",
                    null, 500, "User cannot be remove user");
            }

            return Ok(group.ToGroupResponse());
        }
    }
}
