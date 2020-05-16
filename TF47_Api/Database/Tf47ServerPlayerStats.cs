using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPlayerStats
    {
        public uint PlayerId { get; set; }
        public uint? NumberConnections { get; set; }
        public DateTime? LastTimeSeen { get; set; }
        public uint? TimePlayedInf { get; set; }
        public uint? TimePlayedVehSmall { get; set; }
        public uint? TimePlayedVehTracked { get; set; }
        public uint? TimePlayedVehPlane { get; set; }
        public uint? TimePlayedVehHelo { get; set; }
        public uint? TimePlayedBase { get; set; }
        public uint? TimePlayedObjective { get; set; }
        public uint? KillsInf { get; set; }
        public uint? KillsVehSmall { get; set; }
        public uint? KillsVehTracked { get; set; }
        public uint? KillsVehPlane { get; set; }
        public uint? KillsVehHelo { get; set; }
        public uint? DeathsInf { get; set; }
        public uint? DeathsVehSmall { get; set; }
        public uint? DeathsVehTracked { get; set; }
        public uint? DeathsVehPlane { get; set; }
        public uint? DeathsVehHelo { get; set; }
        public uint? DistanceInf { get; set; }
        public uint? DistanceVehSmall { get; set; }
        public uint? DistanceVehTracked { get; set; }
        public uint? DistanceVehHelo { get; set; }
        public uint? DistanceVehPlane { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
    }
}
