using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TF47_Backend.Database;

namespace TF47_Backend.Services.Authentication
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _tokenExpires;
        private readonly byte[] _secret;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenProvider(ILogger<TokenProvider> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _tokenExpires = int.Parse(configuration["Credentials:JWT:Expires"]);
            _secret = Encoding.ASCII.GetBytes(configuration["Credentials:JWT:Secret"]);
            _tokenHandler = new JwtSecurityTokenHandler();
            logger.LogInformation($"Token provider setup completed. Created tokens will expire after {_tokenExpires} minutes.");
        }

        public async Task<string> GenerateToken(Guid guid)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = await GetClaimsIdentity(guid),
                Expires = DateTime.Now.AddMinutes(_tokenExpires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(Guid guid)
        {
            using var scope = _serviceProvider.CreateScope();
            var databaseContext = scope.ServiceProvider.GetService<DatabaseContext>();
            Debug.Assert(databaseContext != null, nameof(databaseContext) + " != null");
            var user = await databaseContext.Users
                .Include(x => x.UserHasGroups)
                .ThenInclude(x => x.Group)
                .FirstOrDefaultAsync(x => x.UserId == guid);

            if (user == null) return new ClaimsIdentity();

            var claims = new List<Claim>()
            {
                new Claim("Guid", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Mail)
            };


            claims.AddRange(user.UserHasGroups.Select(userGroup => new Claim(ClaimTypes.Role, userGroup.Group.Name)));
            return new ClaimsIdentity(claims);
        }
    }
}
