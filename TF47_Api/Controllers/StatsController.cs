using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.Services;

namespace TF47_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly ILogger<StatsController> _logger;
        private readonly Tf47DatabaseContext _database;
        private readonly GadgetUserProviderService _gadgetUserProviderService;

        public StatsController(ILogger<StatsController> logger, Tf47DatabaseContext database, GadgetUserProviderService gadgetUserProviderService)
        {
            _logger = logger;
            _database = database;
            _gadgetUserProviderService = gadgetUserProviderService;
        }

        [HttpGet("chat")]
        [HttpGet("chat/{page}")]
        public async Task<IActionResult> GetChat(
            int page = 1, 
            [FromQuery(Name = "rows")]int rows = 20,
            [FromQuery(Name = "playerId")]uint? playerId = null,
            [FromQuery(Name = "playerName")]string playerName = null,
            [FromQuery(Name = "side")]string side = null)
        {
            if (page < 1) page = 1;
            page--;

            var gadgetUser = await _gadgetUserProviderService.GetGadgetUserFromHttpContext(HttpContext);

            if (gadgetUser.ForumIsAdmin || gadgetUser.ForumIsModerator)
            {
                if (playerId != null || playerName != null || side != null)
                {
                    return await Task.Run(() =>
                    {
                        var chats = _database.Tf47ServerChatLog
                            .Include(x => x.Player)
                            .Include(x => x.Session)
                            .ThenInclude(x => x.Mission)
                            .Where(x => x.PlayerId == playerId || x.Player.PlayerName == playerName || side == "")
                            .OrderByDescending(x => x.Id)
                            .Skip(rows * page)
                            .Take(rows)
                            .Select(x => new ChatMessage
                            {
                                Id = x.Id,
                                Channel = x.Channel,
                                Message = x.Message,
                                MissionId = x.Session.MissionId,
                                MissionName = x.Session.Mission.MissionName,
                                MissionType = x.Session.Mission.MissionType,
                                PlayerId = x.PlayerId,
                                PlayerName = x.Player.PlayerName,
                                SessionId = x.SessionId,
                                TimeSend = x.TimeSend
                            });

                        var totalChatCount = _database.Tf47ServerChatLog.Count(x => x.PlayerId == playerId || x.Player.PlayerName == playerName || side == "");
                        return Ok(new
                        {
                            TotalChatCount = totalChatCount,
                            Chats = chats
                        });
                    });
                }

                return await Task.Run(() =>
                {
                    var chats = _database.Tf47ServerChatLog
                        .Include(x => x.Player)
                        .Include(x => x.Session)
                        .ThenInclude(x => x.Mission)
                        .OrderByDescending(x => x.Id)
                        .Skip(rows * page)
                        .Take(rows)
                        .Select(x => new ChatMessage
                        {
                            Id = x.Id,
                            Channel = x.Channel,
                            Message = x.Message,
                            MissionId = x.Session.MissionId,
                            MissionName = x.Session.Mission.MissionName,
                            MissionType = x.Session.Mission.MissionType,
                            PlayerId = x.PlayerId,
                            PlayerName = x.Player.PlayerName,
                            SessionId = x.SessionId,
                            TimeSend = x.TimeSend
                        });

                    var totalChatCount = _database.Tf47ServerChatLog.Count(x => x.Id > 0);
                    return Ok(new
                    {
                        TotalChatCount = totalChatCount,
                        Chats = chats
                    });
                });
            }

            return await Task.Run(() =>
            {
                var chats = _database.Tf47ServerChatLog
                    .Include(x => x.Player)
                    .Include(x => x.Session)
                    .ThenInclude(x => x.Mission)
                    .Where(x => x.Channel == "Side" || x.Player.PlayerUid == gadgetUser.PlayerUid)
                    .OrderByDescending(x => x.Id)
                    .Skip(rows * page)
                    .Take(rows)
                    .Select(x => new ChatMessage
                    {
                        Id = x.Id,
                        Channel = x.Channel,
                        Message = x.Message,
                        MissionId = x.Session.MissionId,
                        MissionName = x.Session.Mission.MissionName,
                        MissionType = x.Session.Mission.MissionType,
                        PlayerId = x.PlayerId,
                        PlayerName = x.Player.PlayerName,
                        SessionId = x.SessionId,
                        TimeSend = x.TimeSend
                    });
                var totalChatCount = _database.Tf47ServerChatLog.Count(x => x.Id > 0);
                return Ok(new
                {
                    TotalChatCount = totalChatCount,
                    Chats = chats
                });
            });

        }

        [HttpGet("TicketLog")]
        [HttpGet("TicketLog/{page}")]
        public async Task<IActionResult> GetTicketLog(
            int page = 1,
            [FromQuery(Name = "rows")] int rows = 20)
        {
            if (page < 1) page = 1;
            page--;

            return await Task.Run(() =>
            {
                var ticketLog = _database.Tf47ServerTicketLog
                    .Include(x => x.Session)
                    .ThenInclude(x => x.Mission)
                    .OrderByDescending(x => x.Id)
                    .Skip(rows * page)
                    .Take(rows)
                    .Select(x => new TicketLog
                    {
                        Id = x.Id,
                        TicketChange = x.TicketChange,
                        TicketNow = x.TicketNow,
                        Message = x.Message,
                        TicketChangeTime = x.TicketChangeTime,
                        SessionId = x.SessionId,
                        MissionId = x.Session.MissionId,
                        MissionName = x.Session.Mission.MissionName,
                        MissionType = x.Session.Mission.MissionType
                    });
                var totalTicketCount = _database.Tf47ServerTicketLog.Count(x => x.Id > 0);
                return Ok(new
                {
                    TotalTicketCount = totalTicketCount,
                    TicketLog = ticketLog
                });
            });
        }

        public class TicketLog
        {
            public uint Id { get; set; }
            public int TicketChange { get; set; }
            public uint TicketNow { get; set; }
            public string Message { get; set; }
            public DateTime? TicketChangeTime { get; set; }
            public uint SessionId { get; set; }
            public uint MissionId { get; set; }
            public string MissionName { get; set; }
            public string MissionType { get; set; }
        }


        public class ChatMessage
        {
            public uint Id { get; set; }
            public string Channel { get; set; }
            public string Message { get; set; }
            public DateTime? TimeSend { get; set; }
            public uint PlayerId { get; set; }
            public string PlayerName { get; set; }
            public uint SessionId { get; set; }
            public uint MissionId { get; set; }
            public string MissionName { get; set; }
            public string MissionType { get; set; }
        }
    }
}
