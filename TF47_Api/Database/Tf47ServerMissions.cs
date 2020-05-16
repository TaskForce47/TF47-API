using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerMissions
    {
        public Tf47ServerMissions()
        {
            Tf47ServerSessions = new HashSet<Tf47ServerSessions>();
        }

        public uint Id { get; set; }
        public string MissionType { get; set; }
        public string MissionName { get; set; }

        public virtual ICollection<Tf47ServerSessions> Tf47ServerSessions { get; set; }
    }
}
