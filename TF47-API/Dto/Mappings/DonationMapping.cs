using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class DonationMapping
    {
        public static DonationResponse ToDonationResponse(this Donation data)
        {
            if (data == null) return null;

            var username = data.User == null ? "Anonymous" : data.User.Username;
            return new DonationResponse(data.DonationId, username, data.UserId, data.Note, data.Amount,
                data.TimeOfDonation);
        }

        public static IEnumerable<DonationResponse> ToDonationResponseIEnumerable(this IEnumerable<Donation> data)
        {
            return data?.Select(x => ToDonationResponse(x));
        }
    }
}