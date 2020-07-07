using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PlayerNotesController : ControllerBase
    {
        private readonly ILogger<PlayerNotesController> _logger;
        private readonly Tf47DatabaseContext _database;
        private readonly GadgetUserProviderService _gadgetUserProviderService;

        public PlayerNotesController(ILogger<PlayerNotesController> logger, Tf47DatabaseContext database, GadgetUserProviderService gadgetUserProviderService)
        {
            _logger = logger;
            _database = database;
            _gadgetUserProviderService = gadgetUserProviderService;
        }
        
        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut("AddNote")]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var currentUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            if (currentUser == null) return Unauthorized("missing claims!");
            
            try
            {
                await _database.Tf47GadgetUserNotes.AddAsync(new Tf47GadgetUserNotes
                {
                    AuthorId = currentUser.Id,
                    PlayerId = request.PlayerId,
                    TimeWritten = DateTime.Now,
                    PlayerNote = request.Note,
                    Type = request.Type
                });
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var error = $"Cannot store new note!";
                _logger.LogError($"{error}: {ex.Message}");
                return new ServerError(error);
            }

            return Ok();
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteNote(uint id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var note = await _database.Tf47GadgetUserNotes.FirstOrDefaultAsync(x => x.Id == id);
            if (note == null) return NotFound("note cannot be found!");
            try
            {
                _database.Remove(note);
                await _database.SaveChangesAsync();
                _logger.LogInformation($"Removed note {note.Id} from the database:\ntext: {note.PlayerNote}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot remove note from database: {ex.Message}");
                return new ServerError("cannot remove note from database!");
            }

            return Ok();
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateNote(uint id, [FromBody] UpdateNoteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var note = await _database.Tf47GadgetUserNotes.FirstOrDefaultAsync(x => x.Id == id);
            if (note == null) return NotFound("Note not found!");

            var currentUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            if (currentUser == null) return Unauthorized("missing claims!");

            if (note.AuthorId != currentUser.Id) return BadRequest("you cannot edit someone else notes!");

            note.PlayerNote = request.Note;
            note.LastTimeModified = DateTime.Now;
            note.IsModified = true;
            note.Type = request.Type;

            try
            {
                _database.Update(note);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var error = $"Error while trying to update node {note.Id}";
                _logger.LogError($"{error}: {ex.Message}");
                return new ServerError(error);
            }

            return Ok();
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("getLatest")]
        public async Task<IActionResult> GetLatest()
        {
            var latestNotes = _database.Tf47GadgetUserNotes
                .Include(x => x.Player)
                .Where(x => x.Id > 0)
                .OrderByDescending(x => x.Id)
                .Take(25)
                .Select(x => new
                {
                    Id = x.Id,
                    PlayerName = x.Player.PlayerName,
                    Note = x.PlayerNote,
                    TimeWritten = x.TimeWritten,
                    Author = x.Author.ForumName,
                    Type = x.Type
                });
            return Ok(latestNotes);
        }

        public class AddNoteRequest
        {
            public string Note { get; set; }
            public uint PlayerId { get; set; }
            public string Type { get; set; }
        }

        public class UpdateNoteRequest
        {
            public string Note { get; set; }
            public uint PlayerId { get; set; }
            public string Type { get; set; }
        }
    }
}
