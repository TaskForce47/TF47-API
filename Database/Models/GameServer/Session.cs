using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.GameServer
{
    public class Session
    {
        [Key]
        public long SessionId { get; set; }
        public Mission Mission { get; set; }
        [Required]
        [MaxLength(100)]
        public string WorldName { get; set; }
        public MissionType MissionType { get; set; }
        [Timestamp]
        public DateTime SessionCreated { get; set; }
        [Timestamp]
        public DateTime? SessionEnded { get; set; }
        public ICollection<Kill> Kills { get; set; }
        public ICollection<Playtime> PlayTimes { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}