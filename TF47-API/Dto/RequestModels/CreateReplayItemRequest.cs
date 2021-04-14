namespace TF47_API.Dto.RequestModels
{
    public record CreateReplayItemRequest(long TrackingId, string Type, string Data, float GameTickTime,
        string GameTime);
}