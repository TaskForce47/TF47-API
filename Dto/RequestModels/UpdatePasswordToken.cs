namespace TF47_Backend.Dto.RequestModels
{
    public class UpdatePasswordToken
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}