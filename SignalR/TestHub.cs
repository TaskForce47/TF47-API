using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using TF47_Backend.Database;

namespace TF47_Backend.SignalR
{
    public class TestHub : Hub
    {
        public TestHub()
        {

        }

        public async Task SendHello()
        {
            await Clients.Caller.SendAsync("hello", "This comes from the server", CancellationToken.None);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
