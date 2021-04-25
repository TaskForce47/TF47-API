using System;

namespace TF47_API.Dto.ResponseModels
{
    public record DonationResponse(long DonationId, string Username, Guid? Guid, string Note, decimal Amount,
        DateTime TimeOfDonation);
}