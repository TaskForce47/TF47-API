using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerPlaytimes")]
    public class Playtime
    {
        
        public Player Player { get; set; }
        [Key]
        public string PlayerId { get; set; }

        public Session Session { get; set; }
        public long SessionId { get; set; }

        public TimeSpan TimePlayedInfantry { get; set; }
        public TimeSpan TimePlayedVehicle { get; set; }
        public TimeSpan TimePlayedTank { get; set; }
        public TimeSpan TimePlayedHelicopter { get; set; }
        public TimeSpan TimePlayedFixedWing { get; set; }
        public TimeSpan TimePlayedBoat { get; set; }
        public TimeSpan TimeTrackedObjective { get; set; }
    }
}