using TF47_API.Database.Models;

namespace TF47_API.Dto
{
    public class CreateSessionRequest
    {
        public string WorldName { get; set; }
        public uint MissionId { get; set; }
        public MissionType MissionType { get; set; }
    }
}
