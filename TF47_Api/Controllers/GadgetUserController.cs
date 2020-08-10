using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GadgetUserController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<GadgetUserController> _logger;

        public GadgetUserController(Tf47DatabaseContext database, ILogger<GadgetUserController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(uint id)
        {
            var user = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest("user not found!");

            var response = new GadgetUser
            {
                Id = user.Id,
                AvatarUrl = user.ForumAvatarPath,
                ForumName = user.ForumName,
                PlayerUid = user.PlayerUid,
                IsAdmin = user.ForumIsAdmin,
                IsModerator = user.ForumIsModerator,
                IsSponsor = user.ForumIsSponsor,
                IsTf = user.ForumIsTf
            };
            return Ok(response);
        }
        
        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("getAllGadgetUser")]
        public async Task<IActionResult> GetAllGadgetUser()
        {
            return await Task.Run(() =>
            {
                var response = _database.Tf47GadgetUser
                    .Where(x => x.Id > 0)
                    .Select(x => new GadgetUser
                    {
                        Id = x.Id,
                        AvatarUrl = x.ForumAvatarPath,
                        ForumName = x.ForumName,
                        PlayerUid = x.PlayerUid,
                        IsAdmin = x.ForumIsAdmin,
                        IsModerator = x.ForumIsModerator,
                        IsSponsor = x.ForumIsSponsor,
                        IsTf = x.ForumIsTf
                    });
                return Ok(response);
            });
        }

        public class GadgetUser
        {
            public uint Id { get; set; }
            public string ForumName { get; set; }
            public string AvatarUrl { get; set; }
            public string PlayerUid { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsModerator { get; set; }
            public bool IsSponsor { get; set; }
            public bool IsTf { get; set; }
        }
    }
}
