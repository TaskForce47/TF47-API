using System;
using TF47_API.Database.Models;

namespace TF47_API.Dto.ResponseModels
{
    public record ServerResponse(int ServerID, string Name, string Description, string IP, string Port, string Branch, DateTime LastTimeStarted, GameServerStatus GameServerStatus);
    
}
