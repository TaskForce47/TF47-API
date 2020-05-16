using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerPlayerWhitelisting
    {
        public uint Id { get; set; }
        public uint PlayerId { get; set; }
        public uint WhitelistId { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
        public virtual Tf47ServerWhitelists Whitelist { get; set; }
    }
}
