using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetSquadUser
    {
        public uint Id { get; set; }
        public uint? SquadId { get; set; }
        public uint? UserId { get; set; }
        public string UserSquadNick { get; set; }
        public string UserSquadName { get; set; }
        public string UserSquadEmail { get; set; }
        public string UserSquadIcq { get; set; }
        public string UserSquadRemark { get; set; }

        public virtual Tf47GadgetSquad Squad { get; set; }
        public virtual Tf47GadgetUser User { get; set; }
    }
}
