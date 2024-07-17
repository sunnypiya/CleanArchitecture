using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Domain
{
    public interface IHubService
    {
        Task SendNotificationAsync(string notification);
        Task SendMessageAsync(string taskId, string message);
    }
}
