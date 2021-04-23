using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class WhitelistMapping
    {
        public static WhitelistResponse ToWhitelistResponse(this Whitelist data)
        {
            if (data == null) return null;

            return new WhitelistResponse(data.WhitelistId, data.Name, data.Description);
        }

        public static IEnumerable<WhitelistResponse> ToWhitelistResponseIEnumerable(this IEnumerable<Whitelist> data)
        {
            return data?.Select(x => ToWhitelistResponse(x));
        }
    }
}