using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerTickets")]
    public class Ticket
    {
        public long TicketId { get; set; }
        public Session Session { get; set; }
        public long SessionId { get; set; }
        public Player Player { get; set; } = null;
        public string PlayerUid { get; set; }
        public string Text { get; set; }
        public int NewTicketCount { get; set; }
        public int TicketChangeCount { get; set; }
        
        public DateTime TimeChanged { get; set; }
    }
}