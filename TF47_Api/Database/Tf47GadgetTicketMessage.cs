using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetTicketMessage
    {
        public uint Id { get; set; }
        public string Message { get; set; }
        public uint TicketId { get; set; }
        public uint UserId { get; set; }
        public DateTime? TimeSend { get; set; }

        public virtual Tf47GadgetTicket Ticket { get; set; }
        public virtual Tf47GadgetUser User { get; set; }
    }
}
