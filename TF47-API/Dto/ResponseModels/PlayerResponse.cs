using System;
using System.Collections;
using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record PlayerResponse(string PlayerUid, string PlayerName, DateTime? FirstVisit, DateTime? LastVisit,
        int NumberConnections);

    public record PlayerResponseWithDetails(string PlayerUid, string PlayerName, DateTime? FirstVisit,
        DateTime? LastVisit, int NumberConnections, IEnumerable<ChatResponse> PlayerChats, IEnumerable<NoteResponse> PlayerNotes);
}