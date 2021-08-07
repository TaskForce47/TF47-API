using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Response;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class SlotGroupMapping
    {
        public static SlotGroupResponse ToSlotGroupResponse(this SlotGroup data)
        {
            if (data == null) return null;

            return new SlotGroupResponse(data.SlotGroupId, data.MissionId, data.Title, data.Description, data.OrderNumber, data.Slots.ToSlotResponseIEnumerable());
        }

        public static IEnumerable<SlotGroupResponse> ToSlotGroupResponseIEnumerable(this IEnumerable<SlotGroup> data)
        {
            return data?.Select(x => x.ToSlotGroupResponse());
        }
    }
}