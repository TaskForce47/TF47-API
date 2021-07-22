using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class ServerMapping
    {
        public static ServerResponse ToServerResponse(this Server data)
        {
            if (data == null) return null;

            return new ServerResponse(data.ServerID, data.Name, data.Description, data.IP, data.Port, data.Branch, data.LastTimeStarted, data.GameServerStatus);
        }

        public static IEnumerable<ServerResponse> ToServerResponseIEnumerable(this IEnumerable<Server> data)
        {
            return data?.Select(x => x.ToServerResponse());
        }
    }
}