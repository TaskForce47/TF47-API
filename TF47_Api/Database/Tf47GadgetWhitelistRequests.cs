using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetWhitelistRequests
    {
        public Tf47GadgetWhitelistRequests()
        {
            Tf47GadgetWhitelistMessages = new HashSet<Tf47GadgetWhitelistMessages>();
        }

        public uint Id { get; set; }
        public uint? UserId { get; set; }
        public uint? WhitelistId { get; set; }
        public DateTime? RequestTime { get; set; }
        public string RequestStatus { get; set; }
        public uint? RequestAcceptorId { get; set; }

        public virtual Tf47GadgetUser RequestAcceptor { get; set; }
        public virtual Tf47GadgetUser User { get; set; }
        public virtual Tf47ServerWhitelists Whitelist { get; set; }
        public virtual ICollection<Tf47GadgetWhitelistMessages> Tf47GadgetWhitelistMessages { get; set; }
    }
}
