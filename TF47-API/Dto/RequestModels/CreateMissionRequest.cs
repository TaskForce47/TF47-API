using TF47_API.Database.Models;

namespace TF47_API.Dto.RequestModels
{
    public record CreateMissionRequest(string MissionName, MissionType MissionType, long CampaignId);
}
