using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetActionLog
    {
        public uint Id { get; set; }
        public uint? UserId { get; set; }
        public string Action { get; set; }
        public DateTime? ActionPerformed { get; set; }

        public virtual Tf47GadgetUser User { get; set; }
    }
}
