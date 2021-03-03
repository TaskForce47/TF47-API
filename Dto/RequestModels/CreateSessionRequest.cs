using TF47_Backend.Database.Models;

namespace TF47_Backend.Dto
{
    public class CreateSessionRequest
    {
        public string WorldName { get; set; }
        public uint MissionId { get; set; }
        public MissionType MissionType { get; set; }
    }
}