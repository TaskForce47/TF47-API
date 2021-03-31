using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TF47_API.SignalR
{
    public class ShoutboxHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ShoutboxHub> _logger;
        private readonly List<object> _chatHistory;
        private readonly SemaphoreSlim _lock;

        public ShoutboxHub(IServiceProvider serviceProvider, ILogger<ShoutboxHub> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _chatHistory = new List<object>();
            _lock = new SemaphoreSlim(1);
            _logger.LogInformation("Shoutbox SignalR Hub started!");
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public async Task GetHistory()
        {
            await _lock.WaitAsync();
            _logger.LogInformation($"{Context.ConnectionId} requested chat history");
            await Clients.Caller.SendAsync("history", _chatHistory);
            _lock.Release();
        }

        public async Task SendMessage(string message)
        {
            var user = Context.User?.FindFirst(ClaimTypes.Name);

            if (user == null)
            {
                await Clients.Caller.SendAsync("error", "not authorized");
                return;
            }

            var newMessage = new
            {
                User = user.Value,
                Message = message
            };
            
            _logger.LogInformation($"User {newMessage.User} send message {newMessage.Message}");
            
            await _lock.WaitAsync();
            _chatHistory.Add(newMessage);
            _lock.Release();

            await Clients.All.SendAsync("message", newMessage);
        }
    }
}