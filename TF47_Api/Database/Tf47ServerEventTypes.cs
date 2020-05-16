using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerEventTypes
    {
        public Tf47ServerEventTypes()
        {
            Tf47ServerEventLog = new HashSet<Tf47ServerEventLog>();
        }

        public uint Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Tf47ServerEventLog> Tf47ServerEventLog { get; set; }
    }
}
