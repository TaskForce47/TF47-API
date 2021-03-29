using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.GameServer;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class ChatMapping
    {
        public static IEnumerable<ChatResponse> ToChatResponseIEnumerable(this IEnumerable<Chat> data)
        {
            return data.Select(x =>
                new ChatResponse(x.ChatId, x.Channel, x.Text, x.SessionId, x.PlayerId, x.TimeSend));
        }
    }
}