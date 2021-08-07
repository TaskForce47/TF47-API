using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Response;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class SlotMapping
    {
        public static SlotResponse ToSlotResponse(this Slot data)
        {
            if (data == null) return null;

            return new SlotResponse(data.SlotId, data.SlotGroupId, data.Title, data.Description, data.OrderNumber, data.Difficulty, data.Reserve, data.Blocked, data.RequiredDLC);
        }

        public static IEnumerable<SlotResponse> ToSlotResponseIEnumerable(this IEnumerable<Slot> data)
        {
            return data?.Select(x => x.ToSlotResponse());
        }
    }
}