using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TF47_API.Database.Models;

namespace TF47_API.Dto.Response
{
    public class MissionResponse
    {
        public long MissionId { get; set; }
        public string MissionName { get; set; }
        public MissionType MissionType { get; set; }
        public long CampaignId { get; set; }
        public string CampaignName { get; set; }
    }
}
