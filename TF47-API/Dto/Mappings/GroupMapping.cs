using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class GroupMapping
    {
        public static GroupResponse ToGroupResponse(this Group data)
        {
            return data == null
                ? null
                : new GroupResponse(data.GroupId, data.Name, data.Description, data.TextColor,
                    data.BackgroundColor, data.IsVisible, data.Permissions.ToPermissionResponseIEnumerable());
        }

        public static IEnumerable<GroupResponse> ToGroupResponseIEnumerable(this IEnumerable<Group> data)
        {
            return data?.Select(x => x.ToGroupResponse());
        }
    }
}