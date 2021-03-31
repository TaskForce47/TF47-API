using System;
using System.Collections.Generic;
using TF47_API.Dto.Response;

namespace TF47_API.Dto.ResponseModels
{
    public record CampaignResponse(long CampaignId, string Name, string Description, DateTime TimeCreated,
        IEnumerable<MissionResponse> Missions);
}