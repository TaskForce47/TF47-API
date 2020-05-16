using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerTicketLog
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint TicketNow { get; set; }
        public int TicketChange { get; set; }
        public string Message { get; set; }
        public DateTime? TicketChangeTime { get; set; }

        public virtual Tf47ServerSessions Session { get; set; }
    }
}
