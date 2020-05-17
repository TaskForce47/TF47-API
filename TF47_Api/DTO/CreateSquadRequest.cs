using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class CreateSquadRequest
    {
        [Required]
        public string SquadNick { get; set; }
        [Required]
        public string SquadName { get; set; }
        public string SquadEmail { get; set; }
        public string SquadWeb { get; set; }
        [Required]
        public string SquadTitle { get; set; }
    }
}
