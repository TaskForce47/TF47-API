using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TF47_API.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NotificationHub(
            ILogger<NotificationHub> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task OnConnectedAsync()
        {
            var userGroups = Context.User?.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value);

            if (userGroups != null)
                foreach (var userGroup in userGroups)
                {
                    _logger.LogInformation($"Added user {Context.User.Identity.Name} to notification group {userGroup}");
                    await Groups.AddToGroupAsync(Context.ConnectionId, userGroup, CancellationToken.None);
                }

            await base.OnConnectedAsync();
        }
    }
}