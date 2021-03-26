using System;

namespace TF47_Backend.Dto.ResponseModels
{
    public record ApiKeysResponse(long ApiKeyId, Guid OwnerId, string OwnerUsername, string ApiKey, DateTime TimeCreated, DateTime TimeValidUntil);
}