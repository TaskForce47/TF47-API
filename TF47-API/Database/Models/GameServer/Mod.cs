using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerMods")]
    public class Mod
    {
        [Key]
        public uint ModId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool HasKey { get; set; }
        public ulong FileSize { get; set; }
        public DateTime TimeLastUpdate { get; set; }
        public DateTime TimeInstalled { get; set; }
        public ModStatus ModStatus { get; set; }

        public ICollection<Modset> Modesets { get; set; }
    }
}
