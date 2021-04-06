using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Services;

namespace TF47_API.Controllers.GameServerController
{
    public class WhitelistController : Controller
    {
        private readonly ILogger<WhitelistController> _logger;
        private readonly DatabaseContext _database;
        private readonly IUserProviderService _userProviderService;

        public WhitelistController(
            ILogger<WhitelistController> logger, 
            DatabaseContext database, 
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }
        
        
        
    }
}