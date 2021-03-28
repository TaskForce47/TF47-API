namespace TF47_API.Dto.ResponseModels
{
    public record GroupPermissionsResponse(long GroupPermissionId, string PermissionsDiscord,
        string PermissionsTeamspeak, string PermissionsGadget);
}
