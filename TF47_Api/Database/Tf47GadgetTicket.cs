using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetTicket
    {
        public Tf47GadgetTicket()
        {
            Tf47GadgetTicketMessage = new HashSet<Tf47GadgetTicketMessage>();
        }

        public uint Id { get; set; }
        public uint CategoryId { get; set; }
        public string TicketDescription { get; set; }
        public string TicketStatus { get; set; }
        public uint TicketIssuer { get; set; }
        public DateTime TicketDateCreated { get; set; }
        public bool TicketIsConfidential { get; set; }

        public virtual Tf47GadgetTicketCategory Category { get; set; }
        public virtual Tf47GadgetUser TicketIssuerNavigation { get; set; }
        public virtual ICollection<Tf47GadgetTicketMessage> Tf47GadgetTicketMessage { get; set; }
    }
}
