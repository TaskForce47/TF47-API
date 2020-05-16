using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetSquad
    {
        public Tf47GadgetSquad()
        {
            Tf47GadgetSquadUser = new HashSet<Tf47GadgetSquadUser>();
        }

        public uint Id { get; set; }
        public string SquadNick { get; set; }
        public string SquadName { get; set; }
        public string SquadEmail { get; set; }
        public string SquadWeb { get; set; }
        public string SquadPicture { get; set; }
        public string SquadTitle { get; set; }

        public virtual ICollection<Tf47GadgetSquadUser> Tf47GadgetSquadUser { get; set; }
    }
}
