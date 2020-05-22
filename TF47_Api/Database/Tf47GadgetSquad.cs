using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
        public bool SquadHasPicture { get; set; }
        public string SquadTitle { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tf47GadgetSquadUser> Tf47GadgetSquadUser { get; set; }
    }
}
