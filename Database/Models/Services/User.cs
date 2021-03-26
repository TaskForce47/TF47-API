using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TF47_Backend.Database.Models.GameServer;

namespace TF47_Backend.Database.Models.Services
{
    [Table("ServiceUsers")]
    [Index(nameof(SteamId), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public bool Banned { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        //public string Password { get; set; }
        public string SteamId { get; set; }

        public string CountryCode { get; set; } = null;
        public string ProfilePicture { get; set; }
        public string ProfileUrl { get; set; }
        public string DiscordId { get; set; } = null;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null;

        public bool AllowEmails { get; set; } = false;

        public DateTime FirstTimeSeen { get; set; }
        public DateTime LastTimeSeen { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Note> WrittenNotes { get; set; }
        public ICollection<Changelog> WrittenChangelogs { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; }
    }
}