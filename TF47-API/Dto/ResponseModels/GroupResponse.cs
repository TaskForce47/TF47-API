namespace TF47_API.Dto.ResponseModels
{
    public record GroupResponse(long GroupId, string Name, string Description, string TextColor, string BackgroundColor,
        bool IsVisible, GroupPermissionsResponse GroupPermissions);
}
