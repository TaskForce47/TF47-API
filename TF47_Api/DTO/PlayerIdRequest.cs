using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class PlayerIdRequest
    {
        [Required]
        public uint PlayerId { get; set; }
    }
}
