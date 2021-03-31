using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TF47_API.SignalR
{
    public class ShoutboxHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;
        private List<object> _chatHistory;
        private SemaphoreSlim _lock;

        public ShoutboxHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chatHistory = new List<object>();
            _lock = new SemaphoreSlim(1);
        }

        public async Task GetHistory(CancellationToken cancellationToken)
        {
            await _lock.WaitAsync(cancellationToken);
            await Clients.Caller.SendAsync("history", _chatHistory, cancellationToken);
            _lock.Release();
        }

        public async Task SendMessage(string message, CancellationToken cancellationToken)
        {
            var user = Context.User?.FindFirst(ClaimTypes.Name);

            if (user == null)
            {
                await Clients.Caller.SendAsync("error", "not authorized", cancellationToken: cancellationToken);
                return;
            }

            var newMessage = new
            {
                User = user.Value,
                Message = message
            };
            
            await _lock.WaitAsync(cancellationToken);
            _chatHistory.Add(newMessage);
            _lock.Release();

            await Clients.All.SendAsync("message", newMessage, cancellationToken: cancellationToken);
        }
    }
}