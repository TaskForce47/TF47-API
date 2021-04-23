using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record WhitelistResponse(long WhitelistId, string Name, string Description);

    public record UserWhitelistingResponse(string PlayerUid, string PlayerName, IEnumerable<WhitelistResponse> Whitelistings);
}