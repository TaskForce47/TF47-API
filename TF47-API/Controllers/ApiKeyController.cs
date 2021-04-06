using System;
using System.Buffers;
using System.Buffers.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using crypto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Security;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;
using TF47_API.Services;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : Controller
    {
        private readonly ILogger<ApiKeyController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public ApiKeyController(
            ILogger<ApiKeyController> logger, 
            DatabaseContext database,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        [RequirePermission("apikey:create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiKeyResponse[]), 201)]
        public async Task<IActionResult> CreateApiKey(CreateApiKeyRequest request)
        {
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);

            _database.Attach(user);
            
            var newApikey = new ApiKey
            {
                Description = request.Description,
                TimeCreated = DateTime.Now,
                ValidUntil = request.ValidUntil,
                Owner = user
            };

            var buffer = new byte[2048];
            using var secureRandom = new RNGCryptoServiceProvider();
            using var cryptoProvider = new SHA512CryptoServiceProvider();
            
            secureRandom.GetBytes(buffer);
            for (int i = 0; i < 10000; i++)
            {
                buffer = cryptoProvider.ComputeHash(buffer);
            }

            
            newApikey.ApiKeyValue = Convert.ToBase64String(buffer);
            try
            {
                await _database.ApiKeys.AddAsync(newApikey);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert new Apikey {apikeyid}: {message}", newApikey.ApiKeyId, ex.Message);
                return Problem("Failed to insert new Apikey.", null, 500, "Failed to insert new Apikey");
            }

            return CreatedAtAction(nameof(GetApiKeysUser), new {UserId = newApikey.OwnerId},
                newApikey.ToApiKeyResponse());
        }
        
        [RequirePermission("apikey:view")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiKeyResponse[]), 200)]
        public async Task<IActionResult> GetApiKeys()
        {
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);

            var apiKeys = await _database.ApiKeys
                .AsNoTracking()
                .Include(x => x.Owner)
                .ToListAsync();

            return Ok(apiKeys.ToApiKeyResponseIEnumerable(true));
        }
        
        [HttpGet("{userId:guid}/me")]
        [ProducesResponseType(typeof(ApiKeyResponse[]), 200)]
        public async Task<IActionResult> GetApiKeysUser(Guid userId)
        {
            var apiKeys = await _database.ApiKeys
                .AsNoTracking()
                .Include(x => x.Owner)
                .Where(x => x.OwnerId == userId)
                .ToListAsync();
            
            return Ok(apiKeys.ToApiKeyResponseIEnumerable(true));
        }
        
        [RequirePermission("apikey:view")]
        [HttpGet("{apiKeyId:int}")]
        [ProducesResponseType(typeof(ApiKeyResponse), 200)]
        public async Task<IActionResult> CreateApiKey(long apiKeyId)
        {
            var apiKeys = await _database.ApiKeys
                .AsNoTracking()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.ApiKeyId == apiKeyId);
            
            return Ok(apiKeys.ToApiKeyResponse(true));
        }
        
        [RequirePermission("apikey:edit")]
        [HttpPut("{apiKeyId:int}")]
        [ProducesResponseType(typeof(ApiKeyResponse), 200)]
        public async Task<IActionResult> UpdateApiKey(long apiKeyId, UpdateApiKeyRequest request)
        {
            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);
            var apiKey = await _database.ApiKeys.FirstOrDefaultAsync(x => x.ApiKeyId == apiKeyId);
            if (apiKey == null) return BadRequest("Apikey Id provided does not exist");
            if (apiKey.OwnerId != user.UserId) return BadRequest("You can only edit your own Apikey");
            
            if (request.ValidUntil.HasValue)
                apiKey.ValidUntil = request.ValidUntil.Value;
            if (!string.IsNullOrWhiteSpace(request.Description))
                apiKey.Description = request.Description;

            try
            {
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update api key {keyId}: {message}",apiKeyId, ex.Message);
                return Problem("Failed to update Apikey. Most likely it has been altered or deleted.", null, 500,
                    "Failed to update Apikey");
            }

            return Ok(apiKey.ToApiKeyResponse());
        }
        
        [RequirePermission("apikey:remove")]
        [HttpDelete("{apiKeyId:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveApiKey(long apiKeyId)
        {
            var apiKey = await _database.ApiKeys.FirstOrDefaultAsync(x => x.ApiKeyId == apiKeyId);

            if (apiKey == null) return BadRequest("ApiKey Id provided does not exist");

            try
            {
                _database.Remove(apiKey);
                await _database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete Apikey {apiKeyId}: {message}", apiKeyId, ex.Message);
                return Problem("Failed to delete Apikey. Most likely it has already been deleted.", null, 500,
                    "Failed to delete Apikey");
            }

            return Ok();
        }
        
    }
}