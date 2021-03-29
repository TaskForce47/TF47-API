using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class NoteMapping
    {
        public static IEnumerable<NoteResponse> ToNoteResponseIEnumerable(this IEnumerable<Note> data)
        {
            return data.Select(x => new NoteResponse(x.NoteId, x.Text, x.Type, x.TimeCreated, x.TimeLastUpdate,
                x.PlayerId, x.Player.PlayerName, x.WriterId, x.Writer.Username));
        }
    }
}