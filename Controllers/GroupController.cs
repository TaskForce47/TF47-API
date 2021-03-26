using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Dto.ResponseModels;

namespace TF47_Backend.Controllers
{
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly DatabaseContext _database;

        public GroupController(ILogger<GroupController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }
    
        [HttpGet]
        [ProducesResponseType(typeof(GroupResponse[]), 200)]
        public async Task<IActionResult> GetGroups()
        {
            var result = await Task.Run(() =>
            {
                return _database.Groups
                    .Include(x => x.GroupPermission)
                    .Select(x => new GroupResponse(x.GroupId, x.Name, x.Description, x.TextColor,
                        x.BackgroundColor, x.IsVisible,
                        new GroupPermissionsResponse(x.GroupPermission.GroupPermissionId,
                            x.GroupPermission.PermissionsDiscord, x.GroupPermission.PermissionsTeamspeak,
                            x.GroupPermission.PermissionsGadget)));
            });
            return Ok(result);
        }

        [HttpGet("{groupId:int}")]
        [ProducesResponseType(typeof(GroupResponse), 200)]
        public async Task<IActionResult> GetGroup(int groupId)
        {

            var result = await _database.Groups
                .Include(x => x.GroupPermission)
                .Select(x => new GroupResponse(x.GroupId, x.Name, x.Description, x.TextColor,
                    x.BackgroundColor, x.IsVisible,
                    new GroupPermissionsResponse(x.GroupPermission.GroupPermissionId,
                        x.GroupPermission.PermissionsDiscord, x.GroupPermission.PermissionsTeamspeak,
                        x.GroupPermission.PermissionsGadget)))
                .FirstOrDefaultAsync(x => x.GroupId == groupId);

            if (result == null) return BadRequest("Request group does not exist");

            return Ok(result); 
        }
    }
}