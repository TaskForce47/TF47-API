using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class ChangelogMapping
    {
        public static ChangelogResponse ToChangelogResponse(this Changelog data)
        {
            return data == null
                ? null
                : new ChangelogResponse(data.ChangelogId, data.Title, data.Tags, data.Text, data.TimeReleased);
        }

        public static IEnumerable<ChangelogResponse> ToChangelogResponseIEnumerable(this IEnumerable<Changelog> data)
        {
            return data?.Select(x => ToChangelogResponse(x));
        }
    }
}