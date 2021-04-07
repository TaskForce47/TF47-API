using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;

namespace TF47_API.Services
{
    public class UserProviderService : IUserProviderService
    {
        private readonly ILogger<UserProviderService> _logger;
        private readonly DatabaseContext _database;

        public UserProviderService(
            ILogger<UserProviderService> logger,
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<User> GetDatabaseUserAsync(HttpContext context)
        {
            var userId = Guid.Parse(context.User.Claims.First(claim => claim.Type == "UserId").Value);
            var user = await _database.Users
                .AsNoTracking()
                .FirstAsync(x =>
                x.UserId == userId);
            return user;
        }

        public Guid? GetUserIdByClaimsAsync(HttpContext context)
        {
            if (context.User.Claims.Any(claim => claim.Type == "UserId")) return null;
            return Guid.Parse(context.User.Claims.First(claim => claim.Type == "UserId").Value);
        }
    }
}
