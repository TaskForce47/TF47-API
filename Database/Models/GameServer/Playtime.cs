using System;
using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.GameServer
{
    public class Playtime
    {
        [Key]
        public long PlayTimeId { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public Session Session { get; set; }
        public TimeSpan TimePlayedInfantry { get; set; }
        public TimeSpan TimePlayedVehicle { get; set; }
        public TimeSpan TimePlayedTank { get; set; }
        public TimeSpan TimePlayedHelicopter { get; set; }
        public TimeSpan TimePlayedFixedWing { get; set; }
        public TimeSpan TimePlayedBoat { get; set; }
        public TimeSpan TimeTrackedObjective { get; set; }
    }
}