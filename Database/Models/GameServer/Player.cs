using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;
using TF47_Backend.Database.Models.GameServer;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerPlayers")]
    public class Player
    { 
        [Key]
        [MaxLength(100)]
        public string PlayerUid { get; set; }

        public DateTime? FirstVisit { get; set; } = null;
        public DateTime? LastVisit { get; set; } = null;
        public Playtime PlayerPlaytime { get; set; } = null;

        public ICollection<Kill> PlayerKills { get; set; }
        public ICollection<Kill> PlayerDeaths { get; set; }
        public ICollection<Whitelist> PlayerWhitelistings { get; set; }
        public ICollection<Chat> PlayerChats { get; set; }
        public ICollection<Note> PlayerNotes { get; set; }
    }
}