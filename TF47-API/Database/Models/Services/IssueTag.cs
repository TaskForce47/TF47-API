using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceIssueTags")]
    [Index(nameof(TagName), IsUnique = true)]
    public class IssueTag
    {
        [Key]
        public long IssueTagId { get; set; }
        [MaxLength(100)]
        public string TagName { get; set; }
        [MaxLength(7)]
        public string Color { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
