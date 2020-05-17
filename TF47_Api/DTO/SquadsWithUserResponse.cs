using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.DTO
{
    public class SquadsWithUserResponse
    {
        public uint Id { get; set; }
        public string SquadNick { get; set; }
        public string SquadName { get; set; }
        public string SquadEmail { get; set; }
        public string SquadWeb { get; set; }
        public string SquadPicture { get; set; }
        public string SquadTitle { get; set; }

        public List<SquadUser> SquadUsers { get; set; }
    }
}
