using System;
using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record UserResponse(Guid UserId, bool Banned, string Email, string Username, bool AllowEmails,
        string CountryCode, string DiscordId, string ProfilePicture, string ProfileUrl, string SteamId,
        DateTime FirstTimeSeen, DateTime LastTimeSeen, IEnumerable<NotesResponse> WrittenNotes,
        IEnumerable<ChangelogResponse> WrittenChangelogs, IEnumerable<GroupResponse> UserGroups,
        IEnumerable<ApiKeysResponse> ApiKeys);
}
