using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PTG.NextStep.API.SignalR
{
    public class NotificationHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
