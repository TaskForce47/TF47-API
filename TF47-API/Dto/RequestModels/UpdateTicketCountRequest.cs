namespace TF47_API.Dto.RequestModels
{
    public record UpdateTicketCountRequest(string PlayerUid, int TicketChange, int TicketCountNew, string Message);
}