using System;

namespace TF47_API.Dto
{
    public record UpdateApiKeyRequest(DateTime? ValidUntil, string Description);

    public record CreateApiKeyRequest(string Description, DateTime ValidUntil);
}