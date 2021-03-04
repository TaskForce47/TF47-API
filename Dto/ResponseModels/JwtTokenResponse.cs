using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Backend.Dto.ResponseModels
{
    public class JwtTokenResponse
    {
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
