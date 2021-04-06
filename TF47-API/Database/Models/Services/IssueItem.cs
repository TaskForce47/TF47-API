using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceIssueItems")]
    public class IssueItem
    {
        [Key]
        public long IssueItemId { get; set; }
        [MaxLength(20000)]
        public User Author { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? TimeLastEdited { get; set; }

        public Issue Issue { get; set; }
        public long IssueId { get; set; }
    }
}
