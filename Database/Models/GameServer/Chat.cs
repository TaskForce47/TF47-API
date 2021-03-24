using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerChats")]
    [Index(nameof(Channel))]
    [Index(nameof(Chat.Player))]
    public class Chat
    {
        [Key]
        public long ChatId { get; set; }

        [Required] 
        [MaxLength(20)]
        public string Channel { get; set; }
        [Required]
        [MaxLength(250)]
        public string Text { get; set; }

        public Session Session { get; set; }
        public long SessionId { get; set; }

        public Player Player { get; set; }
        public string PlayerId { get; set; }

        public DateTime TimeSend { get; set; } = DateTime.Now;
    }
}
