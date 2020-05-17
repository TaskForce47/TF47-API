using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class SquadUser
    {
        public uint? UserId { get; set; }
        public string UserSquadNick { get; set; }
        public string UserSquadName { get; set; }
        public string UserSquadEmail { get; set; }
        public string UserSquadIcq { get; set; }
        public string UserSquadRemark { get; set; }
    }
}
