using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TF47_API.Database.Models.GameServer.AAR;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerSessions")]
    public class Session
    {
        [Key]
        public long SessionId { get; set; }

        public Mission Mission { get; set; }
        public long MissionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string WorldName { get; set; }

        public MissionType MissionType { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime? TimeEnded { get; set; } = null;

        public ICollection<Kill> Kills { get; set; }
        public ICollection<Playtime> PlayTimes { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<ReplayItem> ReplayItems { get; set; }
    }
}
