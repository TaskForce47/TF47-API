using System;
using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record UserResponse(Guid UserId, bool Banned, string Email, string Username, bool AllowEmails,
        string CountryCode, string DiscordId, string ProfilePicture, string ProfileUrl, string SteamId,
        DateTime FirstTimeSeen, DateTime LastTimeSeen, IEnumerable<NoteResponse> WrittenNotes,
        IEnumerable<ChangelogResponse> WrittenChangelogs, IEnumerable<GroupResponse> UserGroups,
        IEnumerable<ApiKeyResponse> ApiKeys);

    public record SimpleUserResponse(Guid UserId, bool Banned, string Username, string SteamId, string ProfileUrl);
}
