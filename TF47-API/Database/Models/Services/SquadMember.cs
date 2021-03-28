using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceSquadMembers")]
    public class SquadMember
    {
        public long SquadMemberId { get; set; }
        [MaxLength(128)]
        public string Remark { get; set; }
        [MaxLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public long SquadId { get; set; }
    }
}
