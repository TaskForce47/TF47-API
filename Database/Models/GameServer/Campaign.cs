using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerCampaigns")]
    public class Campaign
    {
        [Key]
        public long CampaignId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public DateTime TimeCreated { get; set; }
        public ICollection<Mission> Missions { get; set; }
    }
}