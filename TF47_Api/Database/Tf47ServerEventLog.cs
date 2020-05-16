using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerEventLog
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint EventId { get; set; }
        public string Message { get; set; }
        public uint? SenderId { get; set; }
        public uint? TargetId { get; set; }

        public virtual Tf47ServerEventTypes Event { get; set; }
        public virtual Tf47ServerPlayers Sender { get; set; }
        public virtual Tf47ServerSessions Session { get; set; }
        public virtual Tf47ServerPlayers Target { get; set; }
    }
}
