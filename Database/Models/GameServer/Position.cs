using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace TF47_Backend.Database.Models.GameServer
{
    public class Position
    {
        [Key]
        public long PositionTrackerId { get; set; }
        public Session Session { get; set; }
        public Player Player { get; set; }
        public NpgsqlPoint Pos { get; set; }
        public Side Side { get; set; }
        public uint Direction { get; set; }
        public uint Velocity { get; set; }
        public string Group { get; set; }
        public VehicleType VehicleType { get; set; }    
        public string VehicleName { get; set; }
        public bool IsDriver { get; set; }
        public bool IsAwake { get; set; }
    }
}