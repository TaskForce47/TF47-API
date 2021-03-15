using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Services
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

        public async Task<User> GetDatabaseUser(HttpContext context)
        {
            var user = await _database.Users.FirstAsync(x =>
                x.UserId == Guid.Parse(context.User.Claims.First(claim => claim.Type == "UserId").Value));
            return user;
        }
    }
}
