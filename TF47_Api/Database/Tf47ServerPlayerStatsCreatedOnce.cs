using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPlayerStatsCreatedOnce
    {
        public uint PlayerId { get; set; }
        public string PlayerNameConnected { get; set; }
        public DateTime? FirstConnectionTime { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
    }
}
