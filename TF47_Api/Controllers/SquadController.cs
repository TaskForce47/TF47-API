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
using TF47_Api.CustomStatusCodes;
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
        public async Task<IActionResult> GetSquadsWithUsers()
        {
            return await Task.Run(() =>
            {
                var response = _database.Tf47GadgetSquad.Include(x => x.Tf47GadgetSquadUser).ThenInclude(y => y.User)
                    .Select(
                        x => new SquadsWithUserResponse
                        {
                            Id = x.Id,
                            SquadEmail = x.SquadEmail,
                            SquadHasPicture = x.SquadHasPicture,
                            SquadName = x.SquadName,
                            SquadNick = x.SquadNick,
                            SquadTitle = x.SquadTitle,
                            SquadWeb = x.SquadWeb,
                            SquadUsers = x.Tf47GadgetSquadUser.Select(z => new SquadUser
                            {
                                UserId = z.UserId,
                                UserSquadEmail = z.UserSquadEmail,
                                UserSquadIcq = z.UserSquadIcq,
                                UserSquadName = z.UserSquadName,
                                UserSquadNick = z.UserSquadNick,
                                UserSquadRemark = z.UserSquadRemark
                            }).ToList()
                        });
                return Ok(response);
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSquad(uint id)
        {
            return await Task.Run(() =>
            {
                var response = _database.Tf47GadgetSquad
                    .Include(x => x.Tf47GadgetSquadUser)
                    .ThenInclude(y => y.User)
                    .Where(x => x.Id == id)
                    .Select(
                        x => new SquadsWithUserResponse
                        {
                            Id = x.Id,
                            SquadEmail = x.SquadEmail,
                            SquadHasPicture = x.SquadHasPicture,
                            SquadName = x.SquadName,
                            SquadNick = x.SquadNick,
                            SquadTitle = x.SquadTitle,
                            SquadWeb = x.SquadWeb,
                            SquadUsers = x.Tf47GadgetSquadUser.Select(z => new SquadUser
                            {
                                UserId = z.UserId,
                                UserSquadEmail = z.UserSquadEmail,
                                UserSquadIcq = z.UserSquadIcq,
                                UserSquadName = z.UserSquadName,
                                UserSquadNick = z.UserSquadNick,
                                UserSquadRemark = z.UserSquadRemark
                            }).ToList()
                        });
                return Ok(response.First(x => x.Id > 0));
            });
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

            try
            {
                await _squadXmlService.RegenerateSquadXmls();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to regenerate squadxml: {ex.Message}");
                return new CustomStatusCodes.ServerError("Failed to regenerate squadxml!");
            }
            
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/update")]
        public async Task<IActionResult> UpdateSquad(uint id, [FromBody] UpdateSquadRequest updateSquadRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == id);
            if (squad == null) return BadRequest("Squad does not exist!");

            squad.SquadEmail = updateSquadRequest.SquadEmail;
            squad.SquadTitle = updateSquadRequest.SquadTitle;
            squad.SquadWeb = updateSquadRequest.SquadWeb;
            squad.SquadNick = updateSquadRequest.SquadNick;

            await _database.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/uploadSquadPicture")]
        public async Task<IActionResult> UploadSquadPicture(uint id, IFormFile data)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == id);
            if (squad == null) return BadRequest("cannot find squad");
            _squadXmlService.DeletePicture(squad);

            if (data.Length < 1) return BadRequest("data corrupted");
            if (data.ContentType != "image/png") return BadRequest("only png images are allowed!");

            var successful = await _squadXmlService.CreatePicture(data, squad);
            if (successful)
            {
                squad.SquadHasPicture = true;
                await _database.SaveChangesAsync();
                await _squadXmlService.GenerateSquadXml(squad.Id);
            }
            else
            {
                return new ServerError("Only png files with a equal height and width are allowed!");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSquad(uint id)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == id);
            if (squad == null) return BadRequest("squad id not found");
            _squadXmlService.DeleteSquadXml(squad);

            _database.Remove(squad);
            await _database.SaveChangesAsync();
            _logger.LogInformation($"Squad {squad.SquadTitle} has been removed!");
            return Ok();
        }

        [HttpGet("{id}/getSquadXml")]
        public async Task<IActionResult> GetSquadXmlUrl(uint id)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == id);
            if (squad == null) return BadRequest("squad id not found");

            var uri = _configuration["ApiListeningUrl"];
            var xmlUrl = $"{uri}squadxml/{squad.SquadNick}/squad.xml";
            return Ok(new
            {
                SquadLink = xmlUrl
            });
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/rebuild")]
        public async Task<IActionResult> RebuildSquadXml(ulong id)
        {
            var squad = await _database.Tf47GadgetSquad.FirstOrDefaultAsync(x => x.Id == id);
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
        [HttpPost("rebuildAllSquadXml")]
        public async Task<IActionResult> RebuildAllSquadXml()
        {
            try
            {
                await _squadXmlService.RegenerateSquadXmls();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while trying to regenerate all squadxmls: {ex.Message}");
                return StatusCode(500, "something went wrong while rebuilding the squad xml");
            }

            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}/addSquadMember")]
        public async Task<IActionResult> AddSquadMember(uint id, [FromBody] AddSquadMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var gadgetUser = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.Id == request.GadgetUserId);
            if (gadgetUser == null) return BadRequest("user does not exist");
            var squadUser = await _database.Tf47GadgetSquadUser.FirstOrDefaultAsync(x =>
                x.UserId == request.GadgetUserId && x.SquadId == id);
            if (squadUser != null) return BadRequest("user already is a member of this squad");

            try
            {
                await _database.Tf47GadgetSquadUser.AddAsync(new Tf47GadgetSquadUser
                {
                    SquadId = id,
                    UserId = request.GadgetUserId,
                    UserSquadIcq = "n/a",
                    UserSquadName = gadgetUser.ForumName,
                    UserSquadEmail = "taskforce47@web.de",
                    UserSquadNick = "n/a",
                    UserSquadRemark = "Teamplay kann man nicht scripten."
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

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("{id}/removeSquadMember")]
        public async Task<IActionResult> RemoveSquadMember(uint id, [FromBody] DeleteSquadMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _database.Tf47GadgetSquadUser.Include(x => x.User).FirstOrDefaultAsync(x =>
                x.SquadId == id && x.UserId == request.GadgetUserId);
            if (user == null) return BadRequest("user cannot be found!");

            try
            {
                _database.Tf47GadgetSquadUser.Remove(user);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var error = $"Error while trying to remove user {user.User.ForumName} from squad {user.UserSquadNick}";
                _logger.LogError("{error}: {ex.Message}");
                return new CustomStatusCodes.ServerError(error);
            }

            try
            {
                if (user.SquadId != null) await _squadXmlService.GenerateSquadXml(user.SquadId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Warning cannot regenerate squadxml {user.UserSquadNick}");
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("{id}/updateSquadMemberDetails")]
        public async Task<IActionResult> UpdateSquadMemberDetails(uint id, [FromBody] UpdateSquadMemberDetails request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _database.Tf47GadgetSquadUser
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                x.SquadId == id && x.UserId == request.GadgetUserId);
            if (user == null) return BadRequest("user does not exist!");

            user.UserSquadEmail = request.Email;
            user.UserSquadRemark = request.Remark;

            try
            {
                _database.Update(user);
            }
            catch (Exception ex)
            {
                var error = $"something went wrong while trying to update {user.User.ForumName} squad xml information!";
                _logger.LogError($"{error}: {ex.Message}");
                return new ServerError(error);
            }

            try
            {
                if (user.SquadId != null) await _squadXmlService.GenerateSquadXml(user.SquadId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Warning cannot regenerate squadxml {user.UserSquadNick}");
            }

            return Ok();
        }

        [HttpGet("{id}/squadMemberCount")]
        public async Task<IActionResult> GetSquadMemberCount(uint id)
        {
            try
            {
                var userCount = await _database.Tf47GadgetSquadUser.Where(x => x.SquadId == id)
                    .SumAsync(x => x.Id);
                return Ok(new
                {
                    SquadId = id,
                    SquadMemberCount = userCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong fetching squad member count: {ex.Message}\nRequest squadid {id}");
                return new ServerError("something went wrong fetching squad member count");
            }
        }
    }

    public class UpdateSquadRequest
    {
        public string SquadNick { get; set; }
        public string SquadTitle { get; set; }
        public string SquadEmail { get; set; }
        public string SquadWeb { get; set; }
    }

    public class UpdateSquadMemberDetails
    {
        public uint GadgetUserId { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
    }

    public class DeleteSquadMemberRequest
    {
        public uint GadgetUserId { get; set; }
    }
}