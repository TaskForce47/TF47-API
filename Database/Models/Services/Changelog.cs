using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.Services
{
    [Table("ServiceChangelogs")]
    public class Changelog
    {
        [Key]
        public long ChangelogId { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public string[] Tags { get; set; }
        [MaxLength(20000)]
        public string Text { get; set; }
        public DateTime TimeReleased { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}