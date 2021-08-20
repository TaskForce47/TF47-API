using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServer")]
    public class Server
    {
        [Key]
        public int ServerID { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public string Port { get; set; }

        public string Branch { get; set; }
        public DateTime LastTimeStarted { get; set; }

        public GameServerStatus GameServerStatus { get; set; }

        public uint ServerConfigID { get; set; } 

        public ServerConfiguration ServerConfiguration { get; set; }

        public uint ModsetId { get; set; }

        public Modset Modset { get; set; }
    }
}
