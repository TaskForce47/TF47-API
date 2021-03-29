using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class ApiKeyMapping
    {
        public static ApiKeyResponse ToApiKeyResponse(this ApiKey data, bool hideApiKey = false)
        {
            if (data == null) return null;

            if (hideApiKey)
            {
                return new ApiKeyResponse(data.ApiKeyId, data.OwnerId, data.Owner.Username, null,
                    data.TimeCreated, data.ValidUntil);
            }
            else
            {
                return new ApiKeyResponse(data.ApiKeyId, data.OwnerId, data.Owner.Username, data.ApiKeyValue,
                    data.TimeCreated, data.ValidUntil);
            }
        }

        public static IEnumerable<ApiKeyResponse> ToApiKeyResponseIEnumerable(this IEnumerable<ApiKey> data,
            bool hideApiKey = false)
        {
            return data?.Select(x => ToApiKeyResponse(x, hideApiKey));
        }
    }
}