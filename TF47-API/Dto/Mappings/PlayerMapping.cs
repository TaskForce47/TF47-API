using System;
using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class PlayerMapping
    {
        public static PlayerResponse ToPlayerResponse(this Player data)
        {
            return data == null
                ? null
                : new PlayerResponse(data.PlayerUid, data.PlayerName, data.TimeFirstVisit, data.TimeLastVisit,
                    data.NumberConnections);
        }

        public static PlayerResponseWithDetails ToPlayerResponseWithDetails(this Player data)
        {
            if (data == null) return null;
            return  new PlayerResponseWithDetails(data.PlayerUid, data.PlayerName, data.TimeFirstVisit, data.TimeLastVisit, data.NumberConnections,
                data.PlayerChats.ToChatResponseIEnumerable(), data.PlayerNotes.ToNoteResponseIEnumerable());
        }
        
        public static IEnumerable<PlayerResponse> ToPlayerResponseIEnumerable(this IEnumerable<Player> data)
        {
            return data?.Select(x => x.ToPlayerResponse());
        }

        public static IEnumerable<PlayerResponseWithDetails> ToPlayerResponseWithDetailsIEnumerable(
            this IEnumerable<Player> data)
        {
            return data?.Select(x => x.ToPlayerResponseWithDetails());
        }
    }
}