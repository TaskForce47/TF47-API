using System;

namespace TF47_Backend.Dto
{
    public class UpdateSessionRequest : CreateSessionRequest
    {
        public DateTime? SessionEnded { get; set; }
    }
}