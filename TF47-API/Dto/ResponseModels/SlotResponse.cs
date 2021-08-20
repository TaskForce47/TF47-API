using System;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Response
{ 
        public record SlotResponse(long SlotId, long SlotGroupId, 
            string Title, string Description, int OrderNumber, int Difficulty, bool Reserve, bool Blocked, bool RequiredDLC, Guid? UserId, UserInfo User);
}
