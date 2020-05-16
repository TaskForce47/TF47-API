using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TF47_Api.Models
{
    public struct AuthenticationUserData
    {
        public DateTime ExpirationDate;
        public ClaimsPrincipal ClaimsPrincipal;
    }
}
