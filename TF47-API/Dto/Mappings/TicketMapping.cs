using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class TicketMapping
    {
        public static TicketResponse ToTicketResponse(this Ticket data)
        {
            if (data == null) return null;

            return new TicketResponse(data.TicketId, data.SessionId, data.Session.MissionId, data.TimeChanged, data.Text,
                data.NewTicketCount, data.TicketChangeCount, data.PlayerUid, data.Player?.PlayerName);
        }

        public static IEnumerable<TicketResponse> ToTicketResponseIEnumerable(this IEnumerable<Ticket> data)
        {
            return data?.Select(x => ToTicketResponse(x));
        }
    }
}