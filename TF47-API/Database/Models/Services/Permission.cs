using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TF47_API.Database.Models.Services.Enums;

namespace TF47_API.Database.Models.Services
{
    [Table("ServicePermissions")]
    [Index(nameof(Permission.Type), IsUnique = false)]
    public class Permission
    {
        [Key]
        public long PermissionId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public PermissionType Type { get; set; }
        public ICollection<Group> GroupPermissions { get; set; }
    }
}