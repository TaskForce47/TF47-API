using System;

namespace TF47_Backend.Dto
{
    public class CreateCampaignRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CampaignResponse : CreateCampaignRequest
    {
        public long CampaignId { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}