using System;

namespace TF47_Backend.Dto.ResponseModels
{
    public record NotesResponse(long NoteId, string PlayerUid, string PlayerName, string Type, string Text,
        Guid WriterId, string Username, DateTime TimeCreated, DateTime? TimeLastUpdate);
}