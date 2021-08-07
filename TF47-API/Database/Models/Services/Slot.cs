using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceSlot")]
    public class Slot
    {
        [Key]
        public long SlotId { get; set; }
        [Required]
        public SlotGroup SlotGroup { get; set; }
        public long SlotGroupId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(20000)]
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public int Difficulty { get; set; }
        public bool Reserve { get; set; }
        public bool Blocked { get; set; }
        public bool RequiredDLC { get; set; }
    }
}
