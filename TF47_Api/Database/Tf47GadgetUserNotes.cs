using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetUserNotes
    {
        public uint Id { get; set; }
        public uint PlayerId { get; set; }
        public string PlayerNote { get; set; }
        public uint AuthorId { get; set; }
        public DateTime TimeWritten { get; set; }
        public string Type { get; set; }

        public virtual Tf47GadgetUser Author { get; set; }
        public virtual Tf47ServerPlayers Player { get; set; }
    }
}
