using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TF47_Backend.Database.Models.Services
{
    public class UserHasGroup
    {
        [Key]
        public long UserHasGroupId { get; set; }
        
        public User User { get; set; }
        public Group Group { get; set; }
        [Timestamp]
        public DateTime TimeAddedToGroup { get; set; }
    }
}