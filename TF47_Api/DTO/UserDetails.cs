namespace TF47_Api.DTO
{
    public class UserDetails
    {
        public uint ForumId { get; set; }
        public string ProfileName { get; set; }
        public string Avatar { get; set; }
        public string[] Roles { get; set; }
        public string PlayerUid { get; set; }
    }
}