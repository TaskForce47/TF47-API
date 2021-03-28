namespace TF47_API.Dto.RequestModels
{
    public class UpdatePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
