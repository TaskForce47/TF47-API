using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_Api.Database;
using TF47_Api.Models;

namespace TF47_Api.Services
{
    public class ClaimProviderService
    {
        private readonly ILogger<ClaimProviderService> _logger;
        private readonly Tf47DatabaseContext _database;

        public ClaimProviderService(ILogger<ClaimProviderService> logger, Tf47DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(ForumUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(CustomClaimTypes.ForumId, user.Uid.ToString())
                //user.Banned ? new Claim(CustomClaimTypes.Banned, "true") : new Claim(CustomClaimTypes.Banned, "false")
            };


            if (user.Groups.FirstOrDefault(group => group.Slug == "administrators") != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            if (user.Groups.FirstOrDefault(group => group.Slug == "global-moderators") != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Moderator"));
            }
            if (user.Groups.FirstOrDefault(group => group.Slug == "task-force-47-mitglied") != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "TF47"));
            }
            if (user.Groups.FirstOrDefault(group => group.Slug == "task-force-47-stammspieler") != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Stammspieler"));
            }
            if (user.Groups.FirstOrDefault(group => group.Slug == "edle-spender") != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Sponsor"));
            }

            claims.Add(string.IsNullOrEmpty(user.Picture)
                ? new Claim(CustomClaimTypes.ProfilePicture, "")
                : new Claim(CustomClaimTypes.ProfilePicture, user.Picture));

            var identity = new ClaimsIdentity(claims, "CustomAuthentication");

            var claimsPrincipal = new ClaimsPrincipal(identity);
            
            var databaseUser = await _database.Tf47GadgetUser.FirstOrDefaultAsync(x => x.ForumId == user.Uid);
            if (databaseUser == null)
            {
                _logger.LogInformation("Creating new gadget user in database");
                var newUser = new Tf47GadgetUser
                {
                    ForumName = user.Username,
                    ForumAvatarPath = user.Picture,
                    ForumId = (uint)user.Uid,
                    ForumLastLogin = DateTime.Now,
                    ForumIsAdmin = claimsPrincipal.IsInRole("Admin"),
                    ForumIsTf = claimsPrincipal.IsInRole("TF47"),
                    ForumIsModerator = claimsPrincipal.IsInRole("Moderator"),
                    ForumIsSponsor = claimsPrincipal.IsInRole("Sponsor"),
                    ForumMail = user.Email
                };
                _database.Tf47GadgetUser.Add(newUser);
                await _database.SaveChangesAsync();
                _logger.LogInformation($"Created new gadget account for {newUser.ForumName}");
            }
            else
            {
                databaseUser = new Tf47GadgetUser
                {
                    ForumName = user.Username,
                    ForumAvatarPath = user.Picture,
                    ForumId = (uint) user.Uid,
                    ForumLastLogin = DateTime.Now,
                    ForumIsAdmin = claimsPrincipal.IsInRole("Admin"),
                    ForumIsTf = claimsPrincipal.IsInRole("TF47"),
                    ForumIsModerator = claimsPrincipal.IsInRole("Moderator"),
                    ForumIsSponsor = claimsPrincipal.IsInRole("Sponsor"),
                    ForumMail = user.Email
                };
                await _database.SaveChangesAsync();
                _logger.LogInformation($"Updated gadget account for user {databaseUser.ForumName}");
            }

            return claimsPrincipal;
        }
    }
}
