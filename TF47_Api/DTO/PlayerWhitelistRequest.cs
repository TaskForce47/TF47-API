using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class PlayerWhitelistRequest
    {
        public uint PlayerId { get; set; }
        public uint WhitelistId { get; set; }
        public bool Enabled { get; set; }
    }
}
