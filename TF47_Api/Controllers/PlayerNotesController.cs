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
        
        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("GetAllPlayerNotes")]
        public async Task<IActionResult> GetAllPlayerNotes()
        {
            return await Task.Run(() =>
            {
                var result = _database.Tf47ServerPlayers
                    .Include(x => x.Tf47GadgetUserNotes)
                    .ThenInclude(x => x.Author)
                    .Where(x => x.Id > 0).Select(x => new PlayerNoteResponse
                    {
                        PlayerId = x.Id,
                        PlayerName = x.PlayerName,
                        Notes = x.Tf47GadgetUserNotes.Select(x => new PlayerNote
                        {
                            NodeId = x.Id,
                            AuthorId = x.AuthorId,
                            AuthorName = x.Author.ForumName,
                            TimeWritten = x.TimeWritten,
                            Note = x.PlayerNote,
                            Type = x.Type
                        }).ToList()
                    });
                return Ok(result);
            });
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet("GetNotesById")]
        public async Task<IActionResult> GetNotesByUserId([FromBody] PlayerIdRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();


            var temp = await _database.Tf47ServerPlayers
                .Include(x => x.Tf47GadgetUserNotes)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == request.PlayerId);

            if (temp == null) return BadRequest("player does not exist!");

            var result = new PlayerNoteResponse
            {
                PlayerId = temp.Id,
                PlayerName = temp.PlayerName,
                Notes = temp.Tf47GadgetUserNotes.Select(x => new PlayerNote
                {
                    NodeId = x.Id,
                    AuthorId = x.AuthorId,
                    AuthorName = x.Author.ForumName,
                    TimeWritten = x.TimeWritten,
                    Note = x.PlayerNote,
                    Type = x.Type
                }).ToList()
            };
            return Ok(result);
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
                _logger.LogError($"Cannot store new note! {ex.Message}");
            }

            return Ok();
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpDelete("DeleteNote")]
        public async Task<IActionResult> DeleteNote([FromBody] DeleteNodeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var note = await _database.Tf47GadgetUserNotes.FirstOrDefaultAsync(x => x.Id == request.NodeId);
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
        [HttpPut("UpdateNote")]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var note = await _database.Tf47GadgetUserNotes.FirstOrDefaultAsync(x => x.Id == request.NodeId);
            if (note == null) return NotFound("Note not found!");

            var currentUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);
            if (currentUser == null) return Unauthorized("missing claims!");

            if (note.AuthorId != currentUser.Id) return BadRequest("you cannot edit someone else notes!");

            note.PlayerNote = request.Note;
            note.PlayerNote += $"\nLast time edited: {DateTime.Now.ToShortDateString()}";
            note.Type = request.Type;

            return Ok();
        }


        public class PlayerNoteResponse
        {
            public uint PlayerId { get; set; }
            public string PlayerName { get; set; }

            public List<PlayerNote> Notes { get; set; }
        }

        public class PlayerNote
        {
            public uint NodeId { get; set; }
            public uint AuthorId { get; set; }
            public string AuthorName { get; set; }
            public DateTime TimeWritten { get; set; }
            public string Note { get; set; }
            public string Type { get; set; }
        }

        public class AddNoteRequest
        {
            public string Note { get; set; }
            public uint PlayerId { get; set; }
            public string Type { get; set; }
        }

        public class UpdateNoteRequest
        {
            public uint NodeId { get; set; }
            public string Note { get; set; }
            public uint PlayerId { get; set; }
            public string Type { get; set; }
        }

        public class DeleteNodeRequest
        {
            public uint NodeId { get; set; }
        }
    }
}
