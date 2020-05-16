using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerKillLog
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint? KillerId { get; set; }
        public uint? VictimId { get; set; }
        public string WeaponName { get; set; }
        public uint Distance { get; set; }
        public string Vehicle { get; set; }

        public virtual Tf47ServerPlayers Killer { get; set; }
        public virtual Tf47ServerSessions Session { get; set; }
        public virtual Tf47ServerPlayers Victim { get; set; }
    }
}
