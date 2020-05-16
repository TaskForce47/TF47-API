using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace TF47_Api.Database
{
    public partial class Tf47ServerWhitelists
    {
        public Tf47ServerWhitelists()
        {
            Tf47GadgetWhitelistRequests = new HashSet<Tf47GadgetWhitelistRequests>();
            Tf47ServerPlayerWhitelisting = new HashSet<Tf47ServerPlayerWhitelisting>();
        }

        public uint Id { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tf47GadgetWhitelistRequests> Tf47GadgetWhitelistRequests { get; set; }
        [JsonIgnore]
        public virtual ICollection<Tf47ServerPlayerWhitelisting> Tf47ServerPlayerWhitelisting { get; set; }
    }
}
