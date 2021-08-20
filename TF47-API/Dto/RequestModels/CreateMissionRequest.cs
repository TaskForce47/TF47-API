using System;
using TF47_API.Database.Models;

namespace TF47_API.Dto.RequestModels
{
    public record CreateMissionRequest(string Name, string DescriptionShort, string? Description, MissionType MissionType,
        long CampaignId, DateTime SlottingTime, DateTime BriefingTime, DateTime StartTime,
        DateTime EndTime, string[] RequiredDLCs);
}
