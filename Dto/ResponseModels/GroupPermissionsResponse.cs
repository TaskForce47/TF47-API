namespace TF47_Backend.Dto.ResponseModels
{
    public record GroupPermissionsResponse(long GroupPermissionId, string PermissionsDiscord,
        string PermissionsTeamspeak, string PermissionsGadget);
}