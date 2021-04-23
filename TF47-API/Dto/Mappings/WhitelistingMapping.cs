using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class WhitelistingMapping
    {
        public static UserWhitelistingResponse ToPlayerWhitelistingResponse(this Player data)
        {
            if (data == null) return null;

            return new UserWhitelistingResponse(data.PlayerUid, data.PlayerName,
                data.PlayerWhitelistings.ToWhitelistResponseIEnumerable());
        }

        public static IEnumerable<UserWhitelistingResponse> ToPlayerWhitelistingResponseIEnumerable(this IEnumerable<Player> data)
        {
            return data?.Select(x => x.ToPlayerWhitelistingResponse());
        }
    }
}