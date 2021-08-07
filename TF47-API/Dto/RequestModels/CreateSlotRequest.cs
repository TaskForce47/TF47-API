namespace TF47_API.Dto.RequestModels
{
    public record CreateSlotRequest(long SlotId, long SlotGroupId, string Title, string Description, int OrderNumber, int Difficulty, bool Reserve, bool Blocked, bool RequiredDLC);
}