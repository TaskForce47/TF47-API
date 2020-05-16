using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerSessions
    {
        public Tf47ServerSessions()
        {
            Tf47ServerChatLog = new HashSet<Tf47ServerChatLog>();
            Tf47ServerEventLog = new HashSet<Tf47ServerEventLog>();
            Tf47ServerKillLog = new HashSet<Tf47ServerKillLog>();
            Tf47ServerPerformance = new HashSet<Tf47ServerPerformance>();
            Tf47ServerPerformanceHc = new HashSet<Tf47ServerPerformanceHc>();
            Tf47ServerPerformancePlayer = new HashSet<Tf47ServerPerformancePlayer>();
            Tf47ServerPositionTracking = new HashSet<Tf47ServerPositionTracking>();
            Tf47ServerTicketLog = new HashSet<Tf47ServerTicketLog>();
        }

        public uint Id { get; set; }
        public uint MissionId { get; set; }
        public string WorldName { get; set; }
        public DateTime? SessionStarted { get; set; }

        public virtual Tf47ServerMissions Mission { get; set; }
        public virtual Tf47ServerTicket Tf47ServerTicket { get; set; }
        public virtual ICollection<Tf47ServerChatLog> Tf47ServerChatLog { get; set; }
        public virtual ICollection<Tf47ServerEventLog> Tf47ServerEventLog { get; set; }
        public virtual ICollection<Tf47ServerKillLog> Tf47ServerKillLog { get; set; }
        public virtual ICollection<Tf47ServerPerformance> Tf47ServerPerformance { get; set; }
        public virtual ICollection<Tf47ServerPerformanceHc> Tf47ServerPerformanceHc { get; set; }
        public virtual ICollection<Tf47ServerPerformancePlayer> Tf47ServerPerformancePlayer { get; set; }
        public virtual ICollection<Tf47ServerPositionTracking> Tf47ServerPositionTracking { get; set; }
        public virtual ICollection<Tf47ServerTicketLog> Tf47ServerTicketLog { get; set; }
    }
}
