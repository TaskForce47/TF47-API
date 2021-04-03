using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;
using TF47_API.Database.Models.GameServer;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerPlayers")]
    public class Player
    { 
        [Key]
        [MaxLength(100)]
        public string PlayerUid { get; set; }
        [MaxLength(100)]
        public string PlayerName { get; set; }

        public DateTime? TimeFirstVisit { get; set; } = null;
        public DateTime? TimeLastVisit { get; set; } = null;

        public int NumberConnections { get; set; } = 0;
        
        public ICollection<Playtime> PlayerPlaytime { get; set; }

        public ICollection<Kill> PlayerKills { get; set; }
        public ICollection<Kill> PlayerDeaths { get; set; }
        public ICollection<Whitelist> PlayerWhitelistings { get; set; }
        public ICollection<Chat> PlayerChats { get; set; }
        public ICollection<Note> PlayerNotes { get; set; }
    }
}
