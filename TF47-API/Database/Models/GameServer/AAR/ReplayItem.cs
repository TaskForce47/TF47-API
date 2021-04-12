using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TF47_API.Database.Models.GameServer.AAR
{
    [Table("GameServerReplayItems")]
    [Index(nameof(SessionId), IsUnique = false)]
    [Index(nameof(TrackingId), IsUnique = false)]
    public class ReplayItem
    {
        [Key]
        public long ReplayItemId { get; set; }

        public Session Session { get; set; }
        public long SessionId { get; set; }

        public long TrackingId { get; set; }
        
        [MaxLength(20)]
        public string Type { get; set; }
        [Column(TypeName = "jsonb")]
        public string Data { get; set; }
        public float GameTickTime { get; set; }
        public string GameTime { get; set; }
        public DateTime TimeTracked { get; set; } = DateTime.Now;
    }
}
