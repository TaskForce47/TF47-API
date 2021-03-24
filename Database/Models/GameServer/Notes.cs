using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Database.Models.GameServer
{
    [Table("GameServerNotes")]
    [Index(nameof(GameServer.Player))]
    public class Note
    {
        [Key]
        public uint NoteId { get; set; }

        [MaxLength(2000)]
        public string Text { get; set; }
        public string Type { get; set; }

        public DateTime TimeCreated { get; set; }
        public DateTime? TimeLastUpdate { get; set; } = null;

        public Player Player { get; set; }
        public string PlayerId { get; set; }

        public User Writer { get; set; }
        public Guid WriterId { get; set; }
    }
}
