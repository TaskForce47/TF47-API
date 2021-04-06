using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Controllers
{
    
    public class PermissionController : Controller
    {
        private readonly DatabaseContext _database;

        public PermissionController(DatabaseContext database)
        {
            _database = database;
        }

        [HttpGet("{permissionId:int}")]
        [ProducesResponseType(typeof(PermissionResponse), 200)]
        public async Task<IActionResult> GetPermission(long permissionId)
        {
            var result = await _database.Permissions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PermissionId == permissionId);

            if (result == null) return BadRequest("PermissionId provided does not exist");

            return Ok(result.ToPermissionResponse());
        }
        
        [HttpGet("")]
        [ProducesResponseType(typeof(GroupResponse[]), 200)]
        public async Task<IActionResult> GetPermissions()
        {
            var result = await _database.Permissions
                .AsNoTracking()
                .ToListAsync();

            if (result == null) return BadRequest("PermissionId provided does not exist");

            return Ok(result.ToPermissionResponseIEnumerable());
        }
    }
}