using System;
using System.Collections.Generic;

namespace TF47_Api.Database
{
    public partial class Tf47GadgetUser
    {
        public Tf47GadgetUser()
        {
            Tf47GadgetActionLog = new HashSet<Tf47GadgetActionLog>();
            Tf47GadgetSquadUser = new HashSet<Tf47GadgetSquadUser>();
            Tf47GadgetTicket = new HashSet<Tf47GadgetTicket>();
            Tf47GadgetTicketMessage = new HashSet<Tf47GadgetTicketMessage>();
            Tf47GadgetUserNotes = new HashSet<Tf47GadgetUserNotes>();
            Tf47GadgetWhitelistMessages = new HashSet<Tf47GadgetWhitelistMessages>();
            Tf47GadgetWhitelistRequestsRequestAcceptor = new HashSet<Tf47GadgetWhitelistRequests>();
            Tf47GadgetWhitelistRequestsUser = new HashSet<Tf47GadgetWhitelistRequests>();
        }

        public uint Id { get; set; }
        public string PlayerUid { get; set; }
        public uint? ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumMail { get; set; }
        public bool ForumIsAdmin { get; set; }
        public bool ForumIsModerator { get; set; }
        public bool? ForumIsSponsor { get; set; }
        public bool? ForumIsTf { get; set; }
        public DateTime? ForumLastLogin { get; set; }
        public string ForumAvatarPath { get; set; }

        public virtual Tf47ServerPlayers PlayerU { get; set; }
        public virtual ICollection<Tf47GadgetActionLog> Tf47GadgetActionLog { get; set; }
        public virtual ICollection<Tf47GadgetSquadUser> Tf47GadgetSquadUser { get; set; }
        public virtual ICollection<Tf47GadgetTicket> Tf47GadgetTicket { get; set; }
        public virtual ICollection<Tf47GadgetTicketMessage> Tf47GadgetTicketMessage { get; set; }
        public virtual ICollection<Tf47GadgetUserNotes> Tf47GadgetUserNotes { get; set; }
        public virtual ICollection<Tf47GadgetWhitelistMessages> Tf47GadgetWhitelistMessages { get; set; }
        public virtual ICollection<Tf47GadgetWhitelistRequests> Tf47GadgetWhitelistRequestsRequestAcceptor { get; set; }
        public virtual ICollection<Tf47GadgetWhitelistRequests> Tf47GadgetWhitelistRequestsUser { get; set; }
    }
}
