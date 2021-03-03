using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Services.Authentication
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        //private readonly HashSet<Guid,User> _userHashSet;

        public UserManager(ILogger<UserManager> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ITokenProvider GetTokenProvider()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetService<ITokenProvider>();
        }
        
        public async Task<AuthenticatedUser> AuthenticateUser(string username, string password)
        {
            //get a database service because it is scoped
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetService<DatabaseContext>();
            //first scan if we have a user with that username or mail
            var user = await database.Users.FirstOrDefaultAsync(x => x.Username == username || x.Mail == username);
            if (user == null) return null; //if we did not manage do find im return null

            //verify password and return an authenticated user handle
            return SecurePasswordHasher.Verify(password, user.Password) ? new AuthenticatedUser(GetTokenProvider(), user.UserId) : null;
        }

        public async Task<AuthenticatedUser> CreateUser(string username, string password, string email)
        {
            //get a database service because it is scoped
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetService<DatabaseContext>();
            //first check if we already have a user with this username or email
            var existingUser = await database.Users.FirstOrDefaultAsync(x => x.Username == username || x.Mail == email);
            if (existingUser != null)
            {
                _logger.LogWarning($"User {existingUser.Username} already exists and cannot be created.");
                throw new UserAlreadyExistException();
            }
            
            //create the new user and salt and hash the password
            var user = new User()
            {
                Banned = false,
                FirstTimeSeen = DateTime.Now,
                LastTimeSeen = DateTime.Now,
                Mail = email,
                Password = SecurePasswordHasher.Hash(password),
                Username = username,
                IsConnectedSteam = false,
                SteamId = null
            };
            try
            {
                await database.Users.AddAsync(user);
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add new user {user.Username} to database.\nError: {ex}");
                return null;
            }

            _logger.LogInformation($"New user {user.Username} with email {user.Mail} has registered.");
            //return a authenticated user handle
            return new AuthenticatedUser(GetTokenProvider(), user.UserId);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            //get a database service because it is scoped
            using var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetService<DatabaseContext>();
            //first scan if we have a user with that guid
            var existingUser = await database.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (existingUser == null) return false;
            
            //now delete him
            try
            {
                database.Remove(existingUser);
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove user {existingUser.Username} from database.\nError: {ex}");
                return false;
            }

            return true;
        }

        public class AuthenticatedUser
        {
            private readonly ITokenProvider _tokenProvider;
            private readonly Guid _guid;

            public AuthenticatedUser(ITokenProvider tokenProvider, Guid guid)
            {
                _tokenProvider = tokenProvider;
                _guid = guid;
            }

            public async Task<string> GetJwtToken()
            {
                return await _tokenProvider.GenerateToken(_guid);
            }
        }

        public static class SecurePasswordHasher
        {
            /// <summary>
            /// Size of salt.
            /// </summary>
            private const int SaltSize = 16;

            /// <summary>
            /// Size of hash.
            /// </summary>
            private const int HashSize = 20;

            /// <summary>
            /// Creates a hash from a password.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="iterations">Number of iterations.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password, int iterations)
            {
                // Create salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

                // Create hash
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                var hash = pbkdf2.GetBytes(HashSize);

                // Combine salt and hash
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Convert to base64
                var base64Hash = Convert.ToBase64String(hashBytes);

                // Format hash with extra information
                return string.Format("$RNG$V1${0}${1}", iterations, base64Hash);
            }

            /// <summary>
            /// Creates a hash from a password with 10000 iterations
            /// </summary>
            /// <param name="password">The password.</param>
            /// <returns>The hash.</returns>
            public static string Hash(string password)
            {
                return Hash(password, 10000);
            }

            /// <summary>
            /// Checks if hash is supported.
            /// </summary>
            /// <param name="hashString">The hash.</param>
            /// <returns>Is supported?</returns>
            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("$RNG$V1$");
            }

            /// <summary>
            /// Verifies a password against a hash.
            /// </summary>
            /// <param name="password">The password.</param>
            /// <param name="hashedPassword">The hash.</param>
            /// <returns>Could be verified?</returns>
            public static bool Verify(string password, string hashedPassword)
            {
                // Check hash
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("The hashtype is not supported");
                }

                // Extract iteration and Base64 string
                var splittedHashString = hashedPassword.Replace("$RNG$V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];

                // Get hash bytes
                var hashBytes = Convert.FromBase64String(base64Hash);

                // Get salt
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Create hash with given salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Get result
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

    public class UserAlreadyExistException : Exception
    {
    }
}
