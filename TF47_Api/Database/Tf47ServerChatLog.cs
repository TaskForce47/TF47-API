using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47ServerChatLog
    {
        public uint Id { get; set; }
        public uint SessionId { get; set; }
        public uint PlayerId { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
        public DateTime? TimeSend { get; set; }

        public virtual Tf47ServerPlayers Player { get; set; }
        public virtual Tf47ServerSessions Session { get; set; }
    }
}
