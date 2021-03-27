using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.Services
{
    [Table("ServiceSquads")]
    public class Squad
    {
        public long SquadId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Website { get; set; }
        [MaxLength(64)]
        public string Nick { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        public string PictureUrl { get; set; } = null;
        
        public ICollection<SquadMember> SquadMembers { get; set; }
    }
}