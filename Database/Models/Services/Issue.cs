﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.Services
{
    public class Issue
    {
        [Key]
        public long IssueId { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public bool IsClosed { get; set; }
        public string[] Tags { get; set; }
        public User IssueCreator { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }
        public IssueGroup IssueGroup { get; set; }
        public long IssueGroupId { get; set; }
    }
}