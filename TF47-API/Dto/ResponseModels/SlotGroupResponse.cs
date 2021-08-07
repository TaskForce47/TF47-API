using System.Collections.Generic;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.Services;

namespace TF47_API.Dto.Response
{ 
        public record SlotGroupResponse(long SlotGroupId, long MissionId, string Title, string Description, int OrderNumber, IEnumerable<SlotResponse> Slots);
}
