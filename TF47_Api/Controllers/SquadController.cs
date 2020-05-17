using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.DTO;

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<SquadController> _logger;

        public SquadController(Tf47DatabaseContext database, ILogger<SquadController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [HttpGet("getSquads")]
        public async Task<IActionResult> GetSquads()
        {
            var squads = _database.Tf47GadgetSquad.Where(x => x.Id > 0);
            return Ok(await squads.ToArrayAsync());
        }

        [HttpGet("getSquadsWithUsers")]
        public async Task<IActionResult> GetSquadsWithUsers()
        {
            var response = new List<SquadsWithUserResponse>();
            var squads = _database.Tf47GadgetSquad.Where(x => x.Id > 0);
            foreach (var squad in squads)
            {
                var squadResponse = new SquadsWithUserResponse
                {
                    Id = squad.Id,
                    SquadEmail = squad.SquadEmail,
                    SquadName = squad.SquadName,
                    SquadNick = squad.SquadNick,
                    SquadPicture = squad.SquadPicture,
                    SquadTitle = squad.SquadTitle,
                    SquadWeb = squad.SquadWeb,
                    SquadUsers = new List<SquadUser>()
                };
                var squadUsers = _database.Tf47GadgetSquadUser.Where(x => x.SquadId == squad.Id);
                foreach (var user in squadUsers)
                {
                    squadResponse.SquadUsers.Add(new SquadUser
                    {
                        UserId = user.UserId,
                        UserSquadEmail = user.UserSquadEmail,
                        UserSquadIcq = user.UserSquadIcq,
                        UserSquadName = user.UserSquadName,
                        UserSquadNick = user.UserSquadNick,
                        UserSquadRemark = user.UserSquadRemark
                    });
                }
            }

            return Ok(response.ToArray());
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut("createSquad")]
        public async Task<IActionResult> CreateSquad([FromBody] CreateSquadRequest createSquadRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _database.Tf47GadgetSquad.AddAsync(new Tf47GadgetSquad
            {
                SquadEmail = createSquadRequest.SquadEmail,
                SquadName = createSquadRequest.SquadName,
                SquadTitle = createSquadRequest.SquadTitle,
                SquadWeb = createSquadRequest.SquadWeb,
                SquadNick = createSquadRequest.SquadNick
            });
            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception e)
            {
               _logger.LogError($"Failed inserting new squad! {e.Message}");
               return StatusCode(500, "something went wrong while trying to insert new squad.");
            }
            return Ok();
        }
    }
}