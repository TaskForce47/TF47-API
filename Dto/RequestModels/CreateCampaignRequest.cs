using System;

namespace TF47_Backend.Dto.RequestModels
{
    public record CreateCampaignRequest(string Name, string Description);

    public record CampaignResponse(string Name, string Description, long CampaignId, DateTime TimeCreated);
}