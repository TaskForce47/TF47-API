using System.Collections.Generic;
using System.Linq;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.Response;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class CampaignMapping
    {
        public static CampaignResponse ToCampaignResponse(this Campaign data)
        {
            return data == null ? 
                null : 
                new CampaignResponse(data.CampaignId, data.Name, data.Description, data.TimeCreated, data.Missions.ToMissionResponsesIEnumerable());
        }

        public static IEnumerable<CampaignResponse> ToCampaignResponseIEnumerable(this IEnumerable<Campaign> data)
        {
            return data?.Select(x => x.ToCampaignResponse());
        }
    }

    public static class MissionMapping
    {
        public static MissionResponse ToMissionResponse(this Mission data)
        {
            if (data == null) return null;

            return new MissionResponse(data.MissionId, data.Name, data.Description, data.MissionType, data.CampaignId,
                data.Campaign.Name);
        }

        public static IEnumerable<MissionResponse> ToMissionResponsesIEnumerable(this IEnumerable<Mission> data)
        {
            return data?.Select(x => x.ToMissionResponse());
        }
    }
}