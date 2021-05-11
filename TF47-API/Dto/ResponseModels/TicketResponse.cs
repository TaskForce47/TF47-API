using System;

namespace TF47_API.Dto.ResponseModels
{
    public record TicketResponse(long TicketId, long SessionId, long MissionId, DateTime TimeChanged, string Text, int TicketCountNew,
        int TicketCountChanged, string PlayerUid, string PlayerName);
}