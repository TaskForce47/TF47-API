using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class PermissionMapping
    {
        public static PermissionResponse ToPermissionResponse(this Permission data)
        {
            if (data == null) return null;

            return new PermissionResponse(data.PermissionId, data.Type, data.Name);
        }

        public static IEnumerable<PermissionResponse> ToPermissionResponseIEnumerable(this IEnumerable<Permission> data)
        {
            return data?.Select(x => x.ToPermissionResponse());
        }
    }
}