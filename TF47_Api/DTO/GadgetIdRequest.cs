using System.ComponentModel.DataAnnotations;

namespace TF47_Api.DTO
{
    public class GadgetIdRequest
    {
        [Required]
        public uint Id { get; set; }
    }
}