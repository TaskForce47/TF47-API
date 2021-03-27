using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TF47_Backend.Database;

namespace TF47_Backend.Controllers
{
    public class SquadController : Controller
    {
        private readonly ILogger<SquadController> _logger;
        private readonly DatabaseContext _database;

        public SquadController(ILogger<SquadController> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }
    }
}