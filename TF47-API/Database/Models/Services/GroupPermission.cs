using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceGroupPermissions")]
    public class GroupPermission
    {
        [Key]
        public long GroupPermissionId { get; set; }

        public string PermissionsDiscord { get; set; } = "0x00000000";
        public string PermissionsTeamspeak { get; set; } = "0x00000000";
        //public string PermissionServerManager { get; set; }
        public string PermissionsGadget { get; set; } = "0x00000000";

        public Group Group { get; set; }
        public long GroupId { get; set; }
    }
}
