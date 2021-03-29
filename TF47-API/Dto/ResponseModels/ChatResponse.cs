using System;

namespace TF47_API.Dto.ResponseModels
{
    public record ChatResponse(long ChatId, string Channel, string Text, long SessionId, string PlayerUid,
        DateTime TimeSend);
}