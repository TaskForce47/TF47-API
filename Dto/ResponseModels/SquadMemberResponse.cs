using System;

namespace TF47_Backend.Dto.ResponseModels
{
    public record SquadMemberResponse(long SquadMemberId, string Remark, string Mail, Guid UserId, string Username,
        string SteamId);

}