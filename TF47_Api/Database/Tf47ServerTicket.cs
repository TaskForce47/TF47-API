using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerTicket
    {
        public uint SessionId { get; set; }
        public uint TicketCount { get; set; }
        public bool SessionFinished { get; set; }

        public virtual Tf47ServerSessions Session { get; set; }
    }
}
