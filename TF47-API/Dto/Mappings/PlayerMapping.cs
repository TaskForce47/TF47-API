using System;
using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class PlayerMapping
    {
        public static PlayerResponse ToPlayerResponse(this Player player)
        {
            return player == null
                ? null
                : new PlayerResponse(player.PlayerUid, player.PlayerName, player.FirstVisit, player.LastVisit,
                    player.NumberConnections);
        }
        
        public static IEnumerable<PlayerResponse> ToPlayerResponseIEnumerable(this IEnumerable<Player> data)
        {
            return data?.Select(x => x.ToPlayerResponse());
        }

        public static IEnumerable<PlayerResponseWithDetails> ToPlayerResponseWithDetailsIEnumerable(
            this IEnumerable<Player> data)
        {
            return data?.Select(x =>
                new PlayerResponseWithDetails(x.PlayerUid, x.PlayerName, x.FirstVisit, x.LastVisit, x.NumberConnections,
                    x.PlayerChats.ToChatResponseIEnumerable(), x.PlayerNotes.ToNoteResponseIEnumerable()));
        }
    }
}