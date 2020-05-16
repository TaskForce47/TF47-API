using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetWhitelistMessages
    {
        public uint Id { get; set; }
        public string Message { get; set; }
        public DateTime? TimeOfMessage { get; set; }
        public uint RequestId { get; set; }
        public uint? MessageAuthor { get; set; }

        public virtual Tf47GadgetUser MessageAuthorNavigation { get; set; }
        public virtual Tf47GadgetWhitelistRequests Request { get; set; }
    }
}
