using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TF47_Api.DTO
{
    public class UserWhitelistResponse
    {
        public uint Id { get; set; }
        public string PlayerName { get; set; }
        public string PlayerUid { get; set; }

        public List<Whitelist> Whitelists { get; set; }
    }

    public class Whitelist
    {
        public uint Id { get; set; }
        public string WhitelistName { get; set; }
        public bool Enabled { get; set; }
        public Whitelist Clone()
        {
            return new Whitelist
            {
                Id = Id,
                WhitelistName = WhitelistName,
                Enabled = Enabled
            };
        }
    }
}
