using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TF47_API.Database.Models.GameServer;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceSlotGroup")]
    public class SlotGroup
    {
        [Key]
        public long SlotGroupId { get; set; }
        [Required]
        public long MissionId { get; set; }
        public Mission Mission { get; set; }
        
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(20000)]
        public string Description { get; set; }
        public int OrderNumber { get; set; }

        public ICollection<Slot> Slots { get; set; }
    }
}
