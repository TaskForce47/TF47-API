namespace TF47_API.Dto.RequestModels
{
    public class UpdatePasswordToken
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
