﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TF47_Backend.Database.Models;

namespace TF47_Backend.Dto
{
    public class CreateMissionRequest
    {
        public string MissionName { get; set; }
        public MissionType MissionType { get; set; }
        public long CampaignId { get; set; }
    }
}
