using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;
using TF47_API.Services;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public NoteController(
            ILogger<NoteController> logger, 
            DatabaseContext database, 
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        [RequirePermission("note:create")]
        [HttpPost("")]
        [ProducesResponseType(typeof(NoteResponse), 200)]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
        {
            var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
            if (player == null) return BadRequest("Provided UserUid does not exist");

            var writer = await _userProviderService.GetDatabaseUserAsync(HttpContext);
            _database.Attach(writer);

            var newNote = new Note
            {
                Player = player,
                Text = request.Text,
                TimeCreated = DateTime.Now,
                Type = request.Type,
                Writer = writer
            };

            try
            {
                await _database.Notes.AddAsync(newNote);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new note to database: {message}", ex.Message);
                return Problem(
                    "Failed to add new note to database. Maybe there is something wrong about the session or properties are set incorrectly",
                    null, 500, "Failed to add new note");
            }

            return CreatedAtAction(nameof(GetNote), new {NoteId = newNote.NoteId}, newNote.ToNoteResponse());
        }

        [RequirePermission("note:update")]
        [HttpPut("{noteId:int}")]
        [ProducesResponseType(typeof(NoteResponse), 200)]
        public async Task<IActionResult> UpdateNote(long noteId, [FromBody] UpdateNoteRequest request)
        {
            var playerNote = await _database.Notes
                .Include(x => x.Writer)
                .Include(x => x.Player)
                .FirstOrDefaultAsync(x => x.NoteId == noteId);
            if (playerNote == null) return BadRequest("Requested note does not exist");
            
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);

            if (user.UserId != playerNote.WriterId)
                return BadRequest("You cannot edit this note! Only the user who created the note can edit it.");

            if (!string.IsNullOrWhiteSpace(request.Text))
                playerNote.Text = request.Text;
            if (!string.IsNullOrWhiteSpace(request.Type))
                playerNote.Type = request.Type;
            
            playerNote.TimeLastUpdate = DateTime.Now;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not update note {noteId}: {message}", noteId, ex.Message);
                return Problem("Could not update note. Maybe someone else deleted or updated at the same time", null,
                    500, "Failed to update note");
            }

            return Ok(playerNote.ToNoteResponse());
        }
        
        [RequirePermission("note:view")]
        [HttpGet("{noteId:int}")]
        [ProducesResponseType(typeof(NoteResponse), 200)]
        public async Task<IActionResult> GetNote(long noteId)
        {
            var playerNote = await _database.Notes
                .AsNoTracking()
                .Include(x => x.Player)
                .Include(x => x.Writer)
                .FirstOrDefaultAsync(x => x.NoteId == noteId);

            if (playerNote == null) return BadRequest("Requested note does not exist");
             
            return Ok(playerNote.ToNoteResponse());
        }
        
        [RequirePermission("note:view")]
        [HttpGet("")]
        [ProducesResponseType(typeof(NoteResponse[]), 200)]
        public async Task<IActionResult> GetNotesPlayer()
        {
            var playerNotes = await Task.Run(() =>
            {
                return _database.Notes
                    .AsNoTracking()
                    .Include(x => x.Player)
                    .Include(x => x.Writer)
                    .OrderByDescending(x => x.NoteId);
            });

            return Ok(playerNotes.AsEnumerable().ToNoteResponseIEnumerable());
        }
        
        [RequirePermission("note:view")]
        [HttpGet("player/{playerUid}")]
        [ProducesResponseType(typeof(NoteResponse[]), 200)]
        public async Task<IActionResult> GetNotesPlayer(string playerUid)
        {
            var playerNotes = await Task.Run(() =>
            {
                return _database.Notes
                    .AsNoTracking()
                    .Include(x => x.Player)
                    .Include(x => x.Writer)
                    .Where(x => x.PlayerId == playerUid);
            });

            return Ok(playerNotes.AsEnumerable().ToNoteResponseIEnumerable());
        }
        
        [RequirePermission("note:remove")]
        [HttpDelete("{noteId:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteNote(long noteId)
        {
            var note = await _database.Notes.FindAsync(noteId);
            if (note == null) return BadRequest("NoteId provided does not match a note");

            try
            {
                _database.Notes.Remove(note);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not delete note from database: {message}", ex.Message);
                return Problem(
                    "Could not delete note from database. Either it is has already been deleted by someone else or data could have been modified",
                    null, 500, "Note could not be deleted");
            }

            return Ok();
        }
    }
}