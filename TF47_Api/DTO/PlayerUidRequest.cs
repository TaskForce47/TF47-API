using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class PlayerUidRequest
    {
        [Required]
        public string PlayerUid { get; set; }
    }
}
