using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerKills")]
    [Index(nameof(Kill.Session))]
    [Index(nameof(Kill.Killer))]
    [Index(nameof(Kill.Victim))]
    public class Kill
    {
        [Key]
        public long KillId { get; set; }

        public Session Session { get; set; }
        public long SessionId { get; set; }

        public Player Killer { get; set; } = null;
        public string KillerId { get; set; } = null;
        
        public VehicleType KillerVehicleType { get; set; }
        public Side KillerSide { get; set; }

        public Player Victim { get; set; } = null;
        public string VictimId { get; set; } = null;

        public VehicleType VictimVehicleType { get; set; }
        public Side VictimSide { get; set; }

        [MaxLength(100)]
        public string Weapon { get; set; }

        [MaxLength(100)] 
        public string VehicleName { get; set; } = null;

        public long Distance { get; set; }
        public long GameTime { get; set; }

        public DateTime RealTime { get; set; }
    }
}
