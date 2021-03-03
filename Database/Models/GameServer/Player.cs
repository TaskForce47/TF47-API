using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;
using TF47_Backend.Database.Models.GameServer;

namespace TF47_Backend.Database.Models
{
    public class Player
    {
        [Key]
        public long PlayerId { get; set; }
        [Required]
        [MaxLength(100)]
        public string PlayerName { get; set; }
        [Required]
        [MaxLength(100)]
        public string PlayerUid { get; set; }
        public DateTime FirstVisit { get; set; }
        public DateTime LastVisit { get; set; }
        public Playtime PlayerPlaytime { get; set; }

        public ICollection<Kill> PlayerKills { get; set; }
        public ICollection<Kill> PlayerDeaths { get; set; }
        public ICollection<Position> PlayerPositions { get; set; }
        public ICollection<Whitelisting> PlayerWhitelistings { get; set; }
        public ICollection<Chat> PlayerChats { get; set; }
    }
}