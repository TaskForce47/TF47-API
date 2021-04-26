using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.GameServer.AAR;

namespace TF47_API.Services.ReplaySystem
{
    public class ReplayService
    {
        private readonly ILogger<ReplayService> _logger;
        private readonly DatabaseContext _database;

        private Session _session;
        private List<ReplayItem> _replayItems;
        
        public ReplayService(ILogger<ReplayService> logger, DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        public async Task<bool> LoadSession(long sessionId)
        {
            var session = await _database.Sessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);

            if (session == null) return false;
            
            _session = session;
            return true;
        }

        public async Task RefreshItems()
        {
            _replayItems = await _database.ReplayItems
                .AsNoTracking()
                .Where(x => x.SessionId == _session.SessionId)
                .ToListAsync();
        }
    }
}