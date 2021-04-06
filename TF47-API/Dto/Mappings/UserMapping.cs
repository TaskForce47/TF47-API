using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class UserMapping
    {
        public static UserResponse ToUserResponse(this User data)
        {
            return data == null
                ? null
                : new UserResponse(data.UserId, data.Banned, data.Email,
                    data.Username, data.AllowEmails, data.CountryCode,
                    data.DiscordId, data.ProfilePicture, data.ProfileUrl, data.SteamId,
                    data.FirstTimeSeen, data.LastTimeSeen);
        }

        public static IEnumerable<UserResponse> ToUserResponseIEnumerable(this IEnumerable<User> data)
        {
            return data?.Select(x => x.ToUserResponse());
        }

        public static SimpleUserResponse ToSimpleUserResponse(this User data)
        {
            return data == null
                ? null
                : new SimpleUserResponse(data.UserId, data.Banned, data.Username, data.SteamId, data.ProfileUrl);
        }

        public static IEnumerable<SimpleUserResponse> ToSimpleUserResponseIEnumerable(this IEnumerable<User> data)
        {
            return data?.Select(x =>  x.ToSimpleUserResponse());
        }
    }
}