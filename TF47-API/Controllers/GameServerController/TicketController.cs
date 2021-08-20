using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;

namespace TF47_API.Controllers.GameServerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly DatabaseContext _database;

        public TicketController(
            ILogger<TicketController> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpPost("{sessionId:int}")]
        public async Task<IActionResult> UpdateTicketCount(long sessionId, [FromBody] UpdateTicketCountRequest request)
        {
            var session = await _database.Sessions
                .Include(x => x.TicketChanges)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);

            var ticketChange = new Ticket
            {
                NewTicketCount = request.TicketCountNew,
                Session = session,
                Text = request.Message,
                TimeChanged = DateTime.Now
            };

            if (!string.IsNullOrWhiteSpace(request.PlayerUid))
            {
                var player = await _database.Players.FirstOrDefaultAsync(x => x.PlayerUid == request.PlayerUid);
                if (player == null) return BadRequest("PlayerUid provided does not exist");
                ticketChange.Player = player;
            }

            await _database.Tickets.AddAsync(ticketChange);
            await _database.SaveChangesAsync();
            
            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTickets([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var tickets = await _database.Tickets
                .AsNoTracking()
                .Include(x => x.Session)
                .ThenInclude(x => x.Mission)
                .Include(x => x.Player)
                .OrderByDescending(x => x.TimeChanged)
                .AsSplitQuery()
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _database.Tickets.CountAsync();
            return Ok(new PagedResponse<IEnumerable<TicketResponse>>(tickets.ToTicketResponseIEnumerable(), validFilter.PageNumber, validFilter.PageSize));
        }

        [HttpGet("{ticketId:long}")]
        public async Task<IActionResult> GetTicket(long ticketId)
        {
            var ticket = await _database.Tickets
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Session)
                .ThenInclude(x => x.Mission)
                .Include(x => x.Player)
                .FirstOrDefaultAsync(x => x.TicketId == ticketId);

            if (ticket == null) return BadRequest("TicketId provided does not exist");

            return Ok(ticket.ToTicketResponse());
        }

        [HttpGet("statistics/topwaster/{year:int}/{month:int}")]
        public async Task<IActionResult> GetTicketWasterOfTheMonth(int year, int month)
        {
            var ticketWaster = await _database.Tickets
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.TimeChanged.Year == year && x.TimeChanged.Month == month)
                .Include(x => x.Session)
                .ThenInclude(x => x.Mission)
                .Include(x => x.Player)
                .GroupBy(x => x.PlayerUid)
                .Select(y => new
                {
                    PlayerUid = y.Key,
                    y.First().Player.PlayerName,
                    Tickets = y.Sum(x => x.TicketChangeCount)
                })
                .OrderByDescending(x => x.Tickets)
                .ToListAsync();

            return Ok(ticketWaster);
        }
        
    }
}