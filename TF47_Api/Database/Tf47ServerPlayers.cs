using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPlayers
    {
        public Tf47ServerPlayers()
        {
            Tf47GadgetUser = new HashSet<Tf47GadgetUser>();
            Tf47GadgetUserNotes = new HashSet<Tf47GadgetUserNotes>();
            Tf47ServerChatLog = new HashSet<Tf47ServerChatLog>();
            Tf47ServerEventLogSender = new HashSet<Tf47ServerEventLog>();
            Tf47ServerEventLogTarget = new HashSet<Tf47ServerEventLog>();
            Tf47ServerKillLogKiller = new HashSet<Tf47ServerKillLog>();
            Tf47ServerKillLogVictim = new HashSet<Tf47ServerKillLog>();
            Tf47ServerPerformancePlayer = new HashSet<Tf47ServerPerformancePlayer>();
            Tf47ServerPlayerWhitelisting = new HashSet<Tf47ServerPlayerWhitelisting>();
            Tf47ServerPositionTracking = new HashSet<Tf47ServerPositionTracking>();
        }

        public string PlayerUid { get; set; }
        public uint Id { get; set; }
        public string PlayerName { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BannedUntil { get; set; }

        public virtual Tf47ServerPlayerStats Tf47ServerPlayerStats { get; set; }
        public virtual Tf47ServerPlayerStatsCreatedOnce Tf47ServerPlayerStatsCreatedOnce { get; set; }
        public virtual ICollection<Tf47GadgetUser> Tf47GadgetUser { get; set; }
        public virtual ICollection<Tf47GadgetUserNotes> Tf47GadgetUserNotes { get; set; }
        public virtual ICollection<Tf47ServerChatLog> Tf47ServerChatLog { get; set; }
        public virtual ICollection<Tf47ServerEventLog> Tf47ServerEventLogSender { get; set; }
        public virtual ICollection<Tf47ServerEventLog> Tf47ServerEventLogTarget { get; set; }
        public virtual ICollection<Tf47ServerKillLog> Tf47ServerKillLogKiller { get; set; }
        public virtual ICollection<Tf47ServerKillLog> Tf47ServerKillLogVictim { get; set; }
        public virtual ICollection<Tf47ServerPerformancePlayer> Tf47ServerPerformancePlayer { get; set; }
        public virtual ICollection<Tf47ServerPlayerWhitelisting> Tf47ServerPlayerWhitelisting { get; set; }
        public virtual ICollection<Tf47ServerPositionTracking> Tf47ServerPositionTracking { get; set; }
    }
}
