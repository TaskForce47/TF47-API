using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.GameServer
{
    public class Whitelisting
    {
        [Key]
        public long WhitelistingId { get; set; }
        public Player Player { get; set; }
        public Whitelist Whitelist { get; set; }
    }
}