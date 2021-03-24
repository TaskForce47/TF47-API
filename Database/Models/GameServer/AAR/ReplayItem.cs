using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.GameServer.AAR
{
    [Table("GameServerReplayItems")]
    public class ReplayItem
    {
        [Key]
        public long ReplayItemId { get; set; }

        public Session Session { get; set; }
        public long SessionId { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }
        [Column(TypeName = "jsonb")]
        public string Data { get; set; }
        public long GameTickTime { get; set; }
        public string GameTime { get; set; }
    }
}