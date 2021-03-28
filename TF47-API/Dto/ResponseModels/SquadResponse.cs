using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record SquadResponse(long SquadId, string Title, string Name, string Nick, string Website, string Mail,
        string SquadXmlLink, string SquadImageLink, IEnumerable<SquadMemberResponse> SquadMembers);
}
