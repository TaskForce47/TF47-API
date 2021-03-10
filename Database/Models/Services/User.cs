﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TF47_Backend.Database.Models.Services
{
    [Index(nameof(SteamId), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public bool Banned { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        //public string Password { get; set; }
        public string SteamId { get; set; }
        public string? CountryCode { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime FirstTimeSeen { get; set; }
        public DateTime LastTimeSeen { get; set; }
        public ICollection<UserHasGroup> UserHasGroups { get; set; }
    }
}