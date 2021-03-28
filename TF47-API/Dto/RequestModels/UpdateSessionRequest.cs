using System;

namespace TF47_API.Dto
{
    public class UpdateSessionRequest : CreateSessionRequest
    {
        public DateTime? SessionEnded { get; set; }
    }
}
