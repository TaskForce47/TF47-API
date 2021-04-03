using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Response;

namespace TF47_API.Dto.Mappings
{
    public static class SessionMapper
    {
        public static SessionResponse ToSessionResponse(this Session data)
        {
            if (data == null) return null;

            return new SessionResponse(data.SessionId, data.MissionId, data.Mission.Name, data.WorldName, data.TimeCreated,
                data.TimeEnded);
        }

        public static IEnumerable<SessionResponse> ToSessionResponseIEnumerable(this IEnumerable<Session> data)
        {
            return data?.Select(x => x.ToSessionResponse());
        }
    }
}