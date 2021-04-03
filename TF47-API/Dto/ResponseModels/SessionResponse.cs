using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_API.Dto.Response
{ 
        public record SessionResponse(long SessionId, long MissionId, string MissionName, string WorldName, DateTime TimeSessionCreated,
        DateTime? TimeSessionEnded);
}
