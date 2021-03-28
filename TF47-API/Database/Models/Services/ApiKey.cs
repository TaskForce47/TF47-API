using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceApiKeys")]
    public class ApiKey
    {
        [Key]
        public long ApiKeyId { get; set; }
        [MaxLength(200)]
        public string ApiKeyValue { get; set; }
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
