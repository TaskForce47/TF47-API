using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.DTO;
using TF47_Api.Models;
using TF47_Api.Services;

namespace TF47_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController : ControllerBase
    {
        private readonly Tf47DatabaseContext _database;
        private readonly ILogger<SquadController> _logger;
        private readonly SquadXmlService _squadXmlService;
        private readonly IConfiguration _configuration;

        public SquadController(Tf47DatabaseContext database, ILogger<SquadController> logger, SquadXmlService squadXmlService, IConfiguration configuration)
        {
            _database = database;
            _logger = logger;
            _squadXmlService = squadXmlService;
            _configuration = configuration;
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
                    SquadHasPicture = squad.SquadHasPicture,
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

            await _squadXmlService.RegenerateSquadXmls();
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("uploadSquadPicture")]
        public async Task<IActionResult> UploadSquadPicture(IFormFile data, [FromBody] SquadIdRequest request)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == request.SquadId);
            if (squad == null) return BadRequest("cannot find squad");
            _squadXmlService.DeletePicture(squad);

            if (data.Length < 1) return BadRequest("data corrupted");

            await _squadXmlService.CreatePicture(data, squad);
            squad.SquadHasPicture = true;
            await _database.SaveChangesAsync();
            await _squadXmlService.GenerateSquadXml(squad.Id);
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("deleteSquad")]
        public async Task<IActionResult> DeleteSquad([FromBody] SquadIdRequest request)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == request.SquadId);
            if (squad == null) return BadRequest("squad id not found");
            _squadXmlService.DeleteSquadXml(squad);

            _database.Remove(squad);
            await _database.SaveChangesAsync();
            _logger.LogInformation($"Squad {squad.SquadTitle} has been removed!");
            return Ok();
        }

        [HttpGet("getSquadXml")]
        public async Task<IActionResult> GetSquadXmlUrl([FromBody] SquadIdRequest request)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == request.SquadId);
            if (squad == null) return BadRequest("squad id not found");

            var uri = _configuration["ApiListeningUrl"];
            return Ok($"{uri}/squadxml/{squad.SquadNick}/squad.xml");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("rebuildSquadXml")]
        public async Task<IActionResult> RebuildSquadXml([FromForm] SquadIdRequest request)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == request.SquadId);
            if (squad == null) return BadRequest("squad id not found");
            try
            {
                await _squadXmlService.GenerateSquadXml(squad.Id);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "something went wrong while rebuilding the squad xml");
            }

            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("rebuildAllSquadXmls")]
        public async Task<IActionResult> RebuildAllSquadXmls()
        {
            try
            {
                await _squadXmlService.RegenerateSquadXmls();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "something went wrong while rebuilding the squad xml");
            }

            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut("addSquadMember")]
        public async Task<IActionResult> AddSquadMember([FromBody] AddSquadMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var gadgetUser = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.Id == request.SquadId);
            if (gadgetUser == null) return BadRequest("user does not exist");
            var squadUser = await _database.Tf47GadgetSquadUser.FirstOrDefaultAsync(x =>
                x.UserId == request.GadgetUserId && x.SquadId == request.SquadId);
            if (squadUser != null) return BadRequest("user already is a member of this squad");

            try
            {
                await _database.Tf47GadgetSquadUser.AddAsync(new Tf47GadgetSquadUser
                {
                    SquadId = request.SquadId,
                    UserId = request.GadgetUserId,
                });
                await _database.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while trying to add new squad member: {e.Message}");
                return StatusCode(500, "something went wrong while trying to insert a new squad member");
            }

            return Ok();
        }

        [HttpGet("squadMemberCount")]
        public async Task<IActionResult> GetSquadMemberCount([FromBody] SquadMemberCountRequest request)
        {
            if (ModelState.IsValid) return BadRequest();
            try
            {
                var userCount = await _database.Tf47GadgetSquadUser.Where(x => x.SquadId == request.SquadId)
                    .SumAsync(x => x.Id);
                return Ok(new
                {
                    SquadId = request.SquadId,
                    SquadMemberCount = userCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong fetching squad member count: {ex.Message}\nRequest squadid {request.SquadId}");
                return StatusCode(500, "something went wrong fetching squad member count");
            }
        }
    }
}