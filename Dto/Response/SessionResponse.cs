using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Backend.Dto.Response
{
    public class SessionResponse
    {
        public long SessionId { get; set; }
        public long MissionId { get; set; }
        public string MissionName { get; set; }
        public DateTime SessionCreated { get; set; }
        public DateTime? SessionEnded { get; set; }
    }
}
