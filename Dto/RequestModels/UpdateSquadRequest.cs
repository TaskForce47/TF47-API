using System;

namespace TF47_Backend.Dto.RequestModels
{
    public record CreateSquadRequest(string Name, string Title, string Nick, string Website, string Mail);
    
    public record UpdateSquadRequest(string Name, string Title, string Nick, string Website, string Mail);
    
    public record CreateSquadMemberRequest(long SquadId, string Remark, string Mail, Guid UserId);

    public record UpdateSquadMemberRequest(string Remark, string Mail);
}