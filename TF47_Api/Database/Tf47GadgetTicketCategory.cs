using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetTicketCategory
    {
        public Tf47GadgetTicketCategory()
        {
            Tf47GadgetTicket = new HashSet<Tf47GadgetTicket>();
        }

        public uint Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<Tf47GadgetTicket> Tf47GadgetTicket { get; set; }
    }
}
