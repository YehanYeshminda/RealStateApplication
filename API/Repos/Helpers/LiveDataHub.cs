using API.Models;
using Microsoft.AspNetCore.SignalR;

namespace API.Repos.Helpers
{
    public class MyHub : Hub
    {
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
