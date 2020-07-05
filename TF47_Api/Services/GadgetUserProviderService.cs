using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.Models;

namespace TF47_Api.Services
{
    public class GadgetUserProviderService
    {
        private readonly ILogger<GadgetUserProviderService> _logger;
        private readonly Tf47DatabaseContext _database;

        public GadgetUserProviderService(ILogger<GadgetUserProviderService> logger, Tf47DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<Tf47GadgetUser> GetGadgetUserFromHttpContext(HttpContext context)
        {
            var forumIdClaim = context.User.Claims.First(x => x.Type == CustomClaimTypes.ForumId).Value;
            if (string.IsNullOrEmpty(forumIdClaim)) return null;

            var currentUser = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.ForumId == uint.Parse(forumIdClaim));
            if (currentUser == null)
            {
                _logger.LogWarning($"This should not be null, claim: {forumIdClaim}");
                return null;
            }

            return currentUser;
        }
    }
}
