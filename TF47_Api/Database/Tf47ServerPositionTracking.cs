using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPositionTracking
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint ServerTime { get; set; }
        public uint PlayerId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public int LookingDir { get; set; }
        public string TravelingMethod { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
        public virtual Tf47ServerSessions Session { get; set; }
    }
}
