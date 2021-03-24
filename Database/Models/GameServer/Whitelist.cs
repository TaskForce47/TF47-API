using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerWhitelists")]
    public class Whitelist
    {
        [Key]
        public long WhitelistId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}