using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;

namespace TF47_API.SignalR
{
    public class EchelonHub : Hub
    {

        private readonly ILogger<EchelonHub> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<RunningSession> _sessionList;
        

        public EchelonHub(ILogger<EchelonHub> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sessionList = new();
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var currentSession = _sessionList.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (currentSession == null) return;
            
            var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            
            var session = await database.Sessions.FirstOrDefaultAsync(x => x.SessionId == currentSession.SessionId);
            if (session != null)
            {
                session.TimeEnded = DateTime.Now;
                try
                {
                    await database.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to stop running session for session Id: {sessionId}", currentSession.SessionId);
                    await Clients.Caller.SendAsync("Error", "failed to save new session");
                }
            }
        }
        
        public async Task UpdateOrCreatePlayer(string playerUid, string playerName)
        {
            var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            
            var player = await database.Players.FirstOrDefaultAsync(x => x.PlayerUid == playerUid);

            if (player == null)
            {
                _logger.LogInformation("New player connected to server! Creating profile for player: {playerName}", playerName);
                player = new Player
                {
                    NumberConnections = 0,
                    PlayerName = playerName,
                    PlayerUid = playerUid,
                    TimeFirstVisit = DateTime.Now,
                    TimeLastVisit = DateTime.Now
                };
                await database.Players.AddAsync(player);
            }
            else
            {
                
                player.PlayerName = playerName;
                player.TimeLastVisit = DateTime.Now;
                player.NumberConnections++;
            }
            
            try
            {
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update playername for {uid}: {message}", playerUid, ex.Message);
                await Clients.Caller.SendAsync("Error", $"Failed to update user {playerName}");
                return;
            }

            await Clients.Caller.SendAsync("playerUpdated", playerName);
        }

        public async Task CreateSession(long missionId, long missionType, string worldName)
        {
            var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var session = new Session
            {
                MissionId = missionId,
                MissionType = (MissionType) missionType,
                TimeCreated = DateTime.Now,
                TimeEnded = null,
                WorldName = worldName
            };

            try
            {
                await database.Sessions.AddAsync(session);
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create new session!: {message}", ex.Message);
                await Clients.Caller.SendAsync("Error", "failed to create new session");
                return;
            }
            
            _logger.LogInformation("Created new session {sessionId}!", session.SessionId);
            _sessionList.Add(new RunningSession
            {
                ConnectionId = Context.ConnectionId,
                SessionId = session.SessionId
            });
            
            await Clients.Caller.SendAsync("sessionCreated", session.SessionId);
        }

        public async Task StopSession(long sessionId)
        {
            var scope = _serviceProvider.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var session = database.Sessions.FirstOrDefault(x => x.SessionId == sessionId);
            if (session == null) return;
            
            session.TimeEnded = DateTime.Now;
            try
            {
                await database.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to stop session {sessionId}: {message}", sessionId, ex.Message);
                await Clients.Caller.SendAsync("Error", "failed to stop session");
                return;
            }

            var sessionTracked = _sessionList.FirstOrDefault(x => x.SessionId == sessionId);
            if (sessionTracked != null)
                _sessionList.Remove(sessionTracked);
            
            await Clients.Caller.SendAsync("sessionStopped", sessionId);
        }

        private class RunningSession
        {
            public string ConnectionId { get; set; }
            public long SessionId { get; set; }
        }
    }
}