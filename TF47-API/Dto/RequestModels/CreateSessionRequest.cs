using System;
using TF47_API.Database.Models;

namespace TF47_API.Dto
{
    public record CreateSessionRequest(long MissionId, string WorldName, MissionType MissionType);

    public record UpdatePlayerNameRequest(string PlayerName);
}
