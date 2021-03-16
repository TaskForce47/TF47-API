using TF47_Backend.Database.Models;

namespace TF47_Backend.Dto.RequestModels
{
    public record CreateMissionRequest(string MissionName, MissionType MissionType, long CampaignId);
}
