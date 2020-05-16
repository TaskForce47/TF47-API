using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPerformance
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint ServerTickTime { get; set; }
        public uint Fps { get; set; }
        public uint RunningSqfSpawned { get; set; }
        public uint RunningSqfExecvm { get; set; }
        public uint RunningFsm { get; set; }
        public uint? ObjCount { get; set; }
        public uint? UnitCount { get; set; }

        public virtual Tf47ServerSessions Session { get; set; }
    }
}
