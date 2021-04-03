using System;

namespace TF47_API.Dto.ResponseModels
{
    public record ApiKeyResponse(long ApiKeyId, string Description, Guid OwnerId, string OwnerUsername,
        string ApiKey, DateTime TimeCreated, DateTime TimeValidUntil);
}
