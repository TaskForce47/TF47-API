using TF47_API.Database.Models.Services.Enums;

namespace TF47_API.Dto.ResponseModels
{
    public record PermissionResponse(long PermissionId, PermissionType Type,
        string Name);
}
