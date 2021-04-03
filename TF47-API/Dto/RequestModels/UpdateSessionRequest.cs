using System;

namespace TF47_API.Dto
{
    public record UpdateSessionRequest(long? MissionId, string WorldName, DateTime? TimeCreated, DateTime? TimeEnded);
}
