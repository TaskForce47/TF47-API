using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.Services
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public bool Banned { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string SteamId { get; set; }
        public bool IsConnectedSteam { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ProfileBackground { get; set; }
        [Timestamp]
        public DateTime FirstTimeSeen { get; set; }
        [Timestamp]
        public DateTime LastTimeSeen { get; set; }
        public ICollection<UserHasGroup> UserHasGroups { get; set; }
    }
}