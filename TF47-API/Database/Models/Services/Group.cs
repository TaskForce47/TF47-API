﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


namespace TF47_API.Database.Models.Services
{
    [Table("ServiceGroups")]
    public class Group 
    {
        [Key]
        public long GroupId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        [Required]
        [MaxLength(7)]
        public string TextColor { get; set; }
        [Required]
        [MaxLength(7)]
        public string BackgroundColor { get; set; }
        [Required]
        public bool IsVisible { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<User> Users { get; set; }
    }

    public class DiscordUser
    {
        //public 
    }

    public class TeamspeakUser
    {
        public long TeamspeakUserId { get; set; }
        public string TemaspeakProfileId { get; set; }
        public User User { get; set; }
        public DateTime TimeProfileConnected { get; set; }
    }
}
