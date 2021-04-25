using System;

namespace TF47_API.Dto.RequestModels
{
    public record CreateDonationRequest(Guid? UserId, decimal Amount, string Note, DateTime TimeOfDonation);

    public record UpdateDonationRequest(Guid? UserId, decimal? Amount, string Note, DateTime? TimeOfDonation);
}