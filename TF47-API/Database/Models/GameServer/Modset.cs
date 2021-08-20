using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerModsets")]
    public class Modset
    {
        [Key]
        public uint ModsetId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }

        public ICollection<Mod> Mods { get; set; }
        public ICollection<Mission> Missions { get; set; }
        public ICollection<Server> Servers { get; set; }
    }
}
