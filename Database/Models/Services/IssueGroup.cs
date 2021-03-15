using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace TF47_Backend.Database.Models.Services
{
    [Index(nameof(GroupName), IsUnique = true)]
    public class IssueGroup
    {
        [Key]
        public long IssueGroupId { get; set; }
        [MaxLength(200)]
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public DateTime TimeGroupCreated { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }
}