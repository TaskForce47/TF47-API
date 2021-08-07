namespace TF47_API.Dto.RequestModels
{
    public record UpdateSlotRequest(long SlotId, string Title, string Description, int OrderNumber, int Difficulty, bool Reserve, bool Blocked, bool RequiredDLC);
}