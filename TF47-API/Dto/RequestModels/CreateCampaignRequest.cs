using System;

namespace TF47_API.Dto.RequestModels
{
    public record CreateCampaignRequest(string Name, string Description);

    public record CampaignResponse(string Name, string Description, long CampaignId, DateTime TimeCreated);
}
