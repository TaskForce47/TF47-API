using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_Backend.Database.Models.Services
{
    public class GroupPermission
    {
        [Key]
        public long GroupPermissionId { get; set; }

        public bool CanEditGroups { get; set; }
        public bool CanBanUsers { get; set; }
        public bool CanBanPermanent { get; set; }
        public bool CanEditUsers { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanEditServers { get; set; }
        public bool CanCreateServers { get; set; }
        public bool CanUseServers { get; set; }

        public Group Group { get; set; }
        public long GroupId { get; set; }
    }
}