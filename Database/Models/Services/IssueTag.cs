using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TF47_Backend.Database.Models.Services
{
    [Index(nameof(TagName), IsUnique = true)]
    public class IssueTag
    {
        public long IssueTagId { get; set; }
        public string TagName { get; set; }
        public string Color { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}
