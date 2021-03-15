using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;
using TF47_Backend.Services.OAuth;

namespace TF47_Backend.Services.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ILogger<AuthenticationManager> _logger;
        private readonly DatabaseContext _database;

        public AuthenticationManager(ILogger<AuthenticationManager> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<ClaimsIdentity> UpdateOrCreateUserAsync(Player steamUser)
        {
            if (steamUser == null)
            {
                _logger.LogWarning($"Failed to update user! steam response returned null");
                return null;
            }

            if (steamUser.Communityvisibilitystate == 1)
                _logger.LogInformation($"{steamUser.Personaname} profile is set to private");

            var user = await _database.Users.FirstOrDefaultAsync(x => x.SteamId == steamUser.Steamid);

            if (user == null)
            {
                _logger.LogInformation($"We don't have a user for {steamUser.Personaname} yet, creating a new user!");
                user = new User
                {
                    Banned = false,
                    CountryCode = steamUser.Loccountrycode,
                    FirstTimeSeen = DateTime.Now,
                    LastTimeSeen = DateTime.Now,
                    Username = steamUser.Personaname,
                    ProfileUrl = steamUser.Profileurl,
                    ProfilePicture = steamUser.Avatarfull,
                    SteamId = steamUser.Steamid
                };
                await _database.AddAsync(user);
            }
            else
            {
                user.CountryCode = steamUser.Loccountrycode;
                user.LastTimeSeen = DateTime.Now;
                user.Username = steamUser.Personaname;
                user.ProfileUrl = steamUser.Profileurl;
                user.ProfilePicture = steamUser.Avatarfull;
                user.SteamId = steamUser.Steamid;
            }

            await _database.SaveChangesAsync();
            return await GetClaimIdentityAsync(user.UserId);
        }

        public async Task<ClaimsIdentity> GetClaimIdentityAsync(Guid guid)
        {
            var user = await _database.Users
                .Include(x => x.Groups)
                .ThenInclude(x => x.GroupPermission)
                .SingleOrDefaultAsync(x => x.UserId == guid);

            //add user details
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Country, user.CountryCode ?? string.Empty),
                new Claim("SteamId", user.SteamId)
            };

            //add user groups and permissions
            claims.AddRange(user.Groups.Select(userGroup => new Claim(ClaimTypes.Role, userGroup.Name)));

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
