using System.Collections.Generic;

namespace TF47_Backend.Dto.ResponseModels
{
    public record SquadResponse(long SquadId, string Name, string Nick, string Website, string Mail,
        string SquadImageLink,
        IEnumerable<SquadMemberResponse> SquadMembers);
}