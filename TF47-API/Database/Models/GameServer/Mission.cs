using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TF47_API.Database.Models.Services;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerMission")]
    public class Mission
    {
        [Key]
        public long MissionId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public string DescriptionShort { get; set; }
        public string Description { get; set; }
        public MissionType MissionType { get; set; }

        public Campaign Campaign { get; set; }
        public long CampaignId { get; set; }

        public ICollection<Session> Sessions { get; set; }
        public ICollection<SlotGroup> SlotGroups { get; set; }

        public DateTime SlottingTime { get; set; }
        public DateTime BriefingTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string[] RequiredDLCs { get; set; }

        public uint ModsetId { get; set; }

        public Modset Modset { get; set; }
    }
}
