namespace TF47_API.Dto.RequestModels
{
    public record CreateUserWhitelistingRequest(string PlayerUid, int WhitelistId);

    public record RemoveUserWhitelistingRequest : CreateUserWhitelistingRequest
    {
        public RemoveUserWhitelistingRequest(string PlayerUid, int WhitelistId) : base(PlayerUid, WhitelistId)
        {
        }
    }
}