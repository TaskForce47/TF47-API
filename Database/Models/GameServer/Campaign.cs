﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.GameServer
{
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