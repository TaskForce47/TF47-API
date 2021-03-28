using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerMission")]
    public class Mission
    {
        [Key]
        public long MissionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public MissionType MissionType { get; set; }

        public Campaign Campaign { get; set; }
        public long CampaignId { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
