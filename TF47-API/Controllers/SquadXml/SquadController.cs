using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Services;
using TF47_API.Services.SquadManager;

namespace TF47_API.Controllers.SquadXml
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController : Controller
    {
        private readonly ILogger<SquadController> _logger;
        private readonly DatabaseContext _database;
        private readonly ISquadManagerService _squadManagerService;
        private readonly IUserProviderService _userProviderService;

        public SquadController(
            ILogger<SquadController> logger,
            DatabaseContext database,
            ISquadManagerService squadManagerService,
            IUserProviderService userProviderService
            )
        {
            _logger = logger;
            _database = database;
            _squadManagerService = squadManagerService;
            _userProviderService = userProviderService;
        }
    
        [HttpGet("{squadId:int}")]
        [ProducesResponseType(typeof(SquadResponse), 200)]
        public async Task<IActionResult> GetSquad(long squadId)
        {
            if (!await _database.Squads.AnyAsync(x => x.SquadId == squadId))
                return BadRequest("Squad requested does not exist");

            var squadResponse = await _database.Squads
                .Include(x => x.SquadMembers)
                .ThenInclude(x => x.User)
                .Where(x => x.SquadId == squadId)
                .Select(x => new SquadResponse(x.SquadId, x.Title, x.Name, x.Nick, x.Website, x.Mail, x.XmlUrl, x.PictureUrl,
                    x.SquadMembers.Select(y => new SquadMemberResponse(y.SquadMemberId, y.Remark, y.Mail, y.UserId,
                        y.User.Username, y.User.SteamId))))
                .FirstOrDefaultAsync();

            return Ok(squadResponse);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SquadResponse[]), 200)]
        public async Task<IActionResult> GetSquads()
        {
            var squadResponse = await Task.Run(() =>
            {
                return _database.Squads
                    .Include(x => x.SquadMembers)
                    .ThenInclude(x => x.User)
                    .Select(x => new SquadResponse(x.SquadId, x.Title,x.Name, x.Nick, x.Website, x.Mail, x.XmlUrl,x.PictureUrl,
                        x.SquadMembers.Select(y => new SquadMemberResponse(y.SquadMemberId, y.Remark, y.Mail, y.UserId,
                            y.User.Username, y.User.SteamId))));
            });

            return Ok(squadResponse);
        }
        
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(SquadMemberResponse[]), 200)]
        public async Task<IActionResult> GetSquadsSelf()
        {
            var user = await _userProviderService.GetDatabaseUser(HttpContext);

            var squads = _database.Squads
                .Include(x => x.SquadMembers)
                .ThenInclude(x => x.User)
                .Where(x => x.SquadMembers.Any(z => z.UserId == user.UserId))
                    .Select(x => new SquadResponse(x.SquadId, x.Title,x.Name, x.Nick, x.Website, x.Mail, x.XmlUrl,x.PictureUrl,
                        x.SquadMembers.Select(y => new SquadMemberResponse(y.SquadMemberId, y.Remark, y.Mail, y.UserId,
                            y.User.Username, y.User.SteamId))));
            
            return Ok(squads);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SquadResponse), 201)]
        public async Task<IActionResult> AddSquad([FromBody] CreateSquadRequest request, CancellationToken cancellationToken)
        {
            var squad = new Squad
            {
                Mail = request.Mail,
                Name = request.Name,
                Nick = request.Nick,
                Title = request.Title,
                Website = request.Website
            };
            try
            {
                await _database.Squads.AddAsync(squad, cancellationToken);
                await _database.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new squad {squadName}: {message}", request.Name, ex.Message);
                return Problem("Failed to create new squad in database", null, 500, "Failed to create Squad");
            }

            try
            {
                await _squadManagerService.WriteSquadXml(squad.SquadId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to generate squadxml for squad {squadName}:{squadId}, {message}", squad.Name,
                    squad.SquadId, ex.Message);
            }

            return CreatedAtAction(nameof(GetSquad), new { SquadId = squad.SquadId },
                new SquadResponse(squad.SquadId, squad.Title, squad.Name, squad.Nick, squad.Website, squad.Mail, squad.XmlUrl,squad.PictureUrl,
                    null));
        }
        
        [HttpPut("{squadId:int}")]
        [ProducesResponseType(typeof(SquadResponse), 200)]
        public async Task<IActionResult> UpdateSquad(long squadId, [FromBody] UpdateSquadRequest request, CancellationToken cancellationToken)
        {
            var squad = await _database.Squads.FindAsync(squadId);
            if (squad == null) return BadRequest("Squad requested does not exist");

            if (!string.IsNullOrWhiteSpace(request.Name))
                squad.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Mail))
                squad.Mail = request.Mail;
            if (!string.IsNullOrWhiteSpace(request.Nick))
                squad.Nick = request.Nick;
            if (!string.IsNullOrWhiteSpace(request.Title))
                squad.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Website))
                squad.Website = request.Website;

            try
            {
                await _database.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update squad {squadName}: {message}", squad.Name, ex.Message);
                return Problem("Failed to update database while trying to save", null, 500, "Failed to updae Squad");
            }
            
            try
            {
                await _squadManagerService.WriteSquadXml(squad.SquadId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update squadxml for squad {squadName}:{squadId}, {message}", squad.Name,
                    squad.SquadId, ex.Message);
            }

            return Ok(new SquadResponse(squad.SquadId, squad.Title, squad.Name, squad.Nick, squad.Website, squad.Mail,
                squad.XmlUrl, squad.PictureUrl, null));
        }
        
        [HttpPut("{squadId:int}/uploadLogo")]
        public async Task<IActionResult> UploadSquadLogo(long squadId, IFormFile file,
            CancellationToken cancellationToken)
        {
            if (file.Length == 0 ||file.Length > 1024*1024*3) return BadRequest("Data size does not look right");
            if (file.ContentType != "image/png") return BadRequest("Only png images are allowed");
            var tempPath = Path.GetTempFileName();
            await using var stream = System.IO.File.Create(tempPath);
            await file.CopyToAsync(stream, cancellationToken);
            stream.Close();
            
            //check if image is valid
            var image = Image.Load(await System.IO.File.ReadAllBytesAsync(tempPath, cancellationToken));
            if ((image.Height & (image.Height - 1)) == 0 && (image.Width & (image.Width - 1)) == 0 &&
                image.Width == image.Height)
            {
                try
                {
                    var result = await _squadManagerService.UpdateSquadImage(squadId, System.IO.File.OpenRead(tempPath),
                        cancellationToken);
                    if (result) return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to update image {message}",ex.Message);
                    return Problem("Failed to update image. Maybe the log will provide more information.", null, 500,
                        "Failed to update image");
                }
            }
            else
            {
                return BadRequest("Image must be a factor of two and have the same dimensions");
            }

            return BadRequest("Something is invalid");
        }
        
        [HttpDelete("{squadId:int}")]
        public async Task<IActionResult> RemoveSquad(long squadId, CancellationToken cancellationToken)
        {
            var squad = await _database.Squads.FindAsync(squadId);
            
            if (squad == null)
                return BadRequest("Squad requested does not exist");

            try
            {
                _database.Squads.Remove(squad);
                await _database.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove squad {squadName}: {message}", squad.Name, ex.Message);
                return Problem("Failed to remove squad from database", null, 500, "Failed to remove Squad");
            }
            
            try
            {
                await _squadManagerService.DeleteSquad(squad.SquadId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update squadxml for squad {squadName}:{squadId}, {message}", squad.Name,
                    squad.SquadId, ex.Message);
            }
    
            return Ok();
        }
        
        [HttpPost("{squadid}/rebuild")]
        public async Task<IActionResult> RebuildSquadXml(long squadid, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _squadManagerService.WriteSquadXml(squadid, cancellationToken);
                if (result)
                    return Ok();
                
                return BadRequest(
                    "Could not complete squadxml rebuild. It is possible the provided squadid does not exist");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to regenerate squadxml for squad {squadId}: {message}", squadid, ex.Message);
                return Problem("Failed to regenerate squadxml", null, 500, "Failed to regenerate squadxml");
            }
        }
    }
}
