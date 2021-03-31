using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TF47_API.Database.Models;

namespace TF47_API.Dto
{ 
    public record UpdateMissionRequest(string Name, string Description, MissionType? MissionType, long? CampaignId);
}
