using System;
using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class SquadMapping
    {
        public static SquadResponse ToSquadResponse(this Squad data)
        {
            return data == null
                ? null
                : new SquadResponse(data.SquadId, data.Title, data.Name, data.Nick, data.Website, data.Mail,
                    data.XmlUrl,
                    data.PictureUrl, data.SquadMembers.ToSquadMemberResponseIEnumerable());
        }
        
        public static IEnumerable<SquadResponse> ToPlayerResponseIEnumerable(this IEnumerable<Squad> data)
        {
            return data?.Select(x => x.ToSquadResponse());
        }

        public static SquadMemberResponse ToSquadMemberResponse(this SquadMember data)
        {
            return data == null
                ? null
                : new SquadMemberResponse(data.SquadMemberId, data.Remark, data.Mail, data.UserId,
                    data.User.Username, data.User.SteamId);
        }
        
        public static IEnumerable<SquadMemberResponse> ToSquadMemberResponseIEnumerable(
            this IEnumerable<SquadMember> data)
        {
            return data?.Select(x => x.ToSquadMemberResponse());
        }
    }
}