using System;

namespace TF47_API.Dto.ResponseModels
{
    public record NoteResponse(long NoteId, string Text, string Type, DateTime TimeCreated, DateTime? TimeLastUpdate,
        string PlayerId, string PlayerName, Guid WriterId, string WriterName);
}