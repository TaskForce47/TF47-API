using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TF47_Backend.Database.Models.GameServer
{
    [Index(nameof(Kill.Session))]
    [Index(nameof(Kill.Killer))]
    [Index(nameof(Kill.Victim))]
    public class Kill
    {
        [Key]
        public long KillId { get; set; }
        public Session Session { get; set; }
        public Player? Killer { get; set; }
        public VehicleType KillerVehicleType { get; set; }
        public Side KillerSide { get; set; }
        public Player? Victim { get; set; }
        public VehicleType VictimVehicleType { get; set; }
        public Side VictimSide { get; set; }
        [MaxLength(100)]
        public string Weapon { get; set; }
        [MaxLength(100)]
        public string? VehicleName { get; set; }
        public long Distance { get; set; }
        public long GameTime { get; set; }
        [Timestamp]
        public DateTime RealTime { get; set; }
    }
}