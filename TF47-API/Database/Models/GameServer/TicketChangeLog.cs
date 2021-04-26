namespace TF47_API.Database.Models.GameServer
{
    public class TicketChangeLog
    {
        public long TicketChangeId { get; set; }
        
        public Session Session { get; set; }
        public long SessionId { get; set; }
        
        public Player Player { get; set; }
        public string PlayerUid { get; set; }
        
        public string Text { get; set; }
        
        public int TicketChange { get; set; }
    }
}