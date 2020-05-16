using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.Models
{
    public class UserWhitelist
    {
        public uint PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerUid { get; set; }
        public List<Whitelist> Whitelists { get; set; }

    }

    public class Whitelist
    {
        public uint Id { get; set; }
        public string WhitelistName { get; set; }
        public bool Enabled { get; set; }
    }
}
