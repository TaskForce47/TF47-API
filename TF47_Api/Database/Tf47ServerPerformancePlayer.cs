using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPerformancePlayer
    {
        public uint Id { get; set; }
        public uint PlayerId { get; set; }
        public uint SessionId { get; set; }
        public uint ServerTickTime { get; set; }
        public uint Fps { get; set; }
        public uint RunningSqfSpawned { get; set; }
        public uint RunningSqfExecvm { get; set; }
        public uint RunningFsm { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
        public virtual Tf47ServerSessions Session { get; set; }
    }
}
