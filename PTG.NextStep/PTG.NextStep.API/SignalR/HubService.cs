using Microsoft.AspNetCore.SignalR;
using PTG.NextStep.API.SignalR;
using PTG.NextStep.Domain;
using System.Threading.Tasks;

namespace PTG.NextStep.Service
{
    public class HubService:IHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public HubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Method to send a notification to all clients
        public async Task SendNotificationAsync(string notification)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        // Method to send a message to a specific client
        public async Task SendMessageAsync(string taskId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", taskId, message);
        }
    }
}
