using System.Collections.Generic;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.Services;

namespace TF47_API.Dto.Response
{ 
        public record SlotResponse(long SlotId, long SlotGroupId, 
            string Title, string Description, int OrderNumber, int Difficulty, bool Reserve, bool Blocked, bool RequiredDLC);
}
