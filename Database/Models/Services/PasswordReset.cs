using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Backend.Database.Models.Services
{
    public class PasswordReset
    {
        [Key]
        public long PasswordResetId { get; set; }
        public string ResetToken { get; set; }
        public DateTime TimePasswordResetGenerated { get; set; }
        public User User { get; set; }
    }
}
