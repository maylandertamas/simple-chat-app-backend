using Microsoft.AspNetCore.SignalR;
using SnBackendApp.Entities;
using System.Threading.Tasks;

namespace SnBackendApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
