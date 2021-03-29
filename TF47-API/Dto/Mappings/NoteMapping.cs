using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class NoteMapping 
    {
        public static NoteResponse ToNoteResponse(this Note data)
        {
            return data == null
                ? null
                : new NoteResponse(data.NoteId, data.Player.PlayerUid, data.Player.PlayerName, data.TimeCreated,
                    data.TimeLastUpdate, data.PlayerId, data.Player.PlayerName, data.WriterId, data.Writer.Username);
        }
        
        public static IEnumerable<NoteResponse> ToNoteResponseIEnumerable(this IEnumerable<Note> data)
        {
            return data?.Select(x => ToNoteResponse(x));
        }
    }
}